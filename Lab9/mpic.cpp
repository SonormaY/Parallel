#include <mpi.h>
#include <iostream>
#include <vector>
#include <ctime>
#include <cstdlib>

const int FRAGMENT_SIZE = 5;

void generateMatrix(std::vector<std::vector<int>>& matrix, int N) {
    for (int i = 0; i < N; ++i) {
        for (int j = 0; j < N; ++j) {
            matrix[i][j] = rand() % 10;
        }
    }
}

void printFragment(const std::vector<std::vector<int>>& matrix, int N) {
    for (int i = 0; i < FRAGMENT_SIZE && i < N; ++i) {
        for (int j = 0; j < FRAGMENT_SIZE && j < N; ++j) {
            std::cout << matrix[i][j] << " ";
        }
        std::cout << std::endl;
    }
}

int main(int argc, char** argv) {
    MPI_Init(&argc, &argv);

    int rank, size;
    MPI_Comm_rank(MPI_COMM_WORLD, &rank);
    MPI_Comm_size(MPI_COMM_WORLD, &size);

    int N;
    if (rank == 0) {
        std::cout << "Enter the size of the matrix (N): ";
        std::cin >> N;
    }

    // Розсилка розміру матриці всім процесам
    MPI_Bcast(&N, 1, MPI_INT, 0, MPI_COMM_WORLD);

    std::vector<std::vector<int>> A(N, std::vector<int>(N));
    std::vector<std::vector<int>> B(N, std::vector<int>(N));
    std::vector<std::vector<int>> C(N, std::vector<int>(N, 0));

    if (rank == 0) {
        srand(time(0));
        generateMatrix(A, N);
        generateMatrix(B, N);

        std::cout << "Matrix A (fragment 5x5):" << std::endl;
        printFragment(A, N);
        std::cout << "\nMatrix B (fragment 5x5):" << std::endl;
        printFragment(B, N);
    }

    // Розсилка матриць A і B всім процесам
    MPI_Bcast(&A[0][0], N * N, MPI_INT, 0, MPI_COMM_WORLD);
    MPI_Bcast(&B[0][0], N * N, MPI_INT, 0, MPI_COMM_WORLD);

    // Кількість рядків, яку буде обробляти кожен процес
    int rows_per_proc = N / size;
    int remainder = N % size; // Залишок рядків для обробки
    int start_row = rank * rows_per_proc + std::min(rank, remainder);
    int end_row = start_row + rows_per_proc + (rank < remainder ? 1 : 0);

    double start_time = MPI_Wtime();

    // Множення матриць (кожен процес обробляє свою частину рядків)
    for (int i = start_row; i < end_row; ++i) {
        for (int j = 0; j < N; ++j) {
            for (int k = 0; k < N; ++k) {
                C[i][j] += A[i][k] * B[k][j];
            }
        }
    }

    double end_time = MPI_Wtime();
    double local_elapsed = end_time - start_time;

    // Відправлення часу виконання кожного процесу на процес 0
    double max_elapsed;
    MPI_Reduce(&local_elapsed, &max_elapsed, 1, MPI_DOUBLE, MPI_MAX, 0, MPI_COMM_WORLD);

    // Збір результатів у процесі з рангом 0
    if (rank == 0) {
        // Отримання результатів від інших процесів
        for (int proc = 1; proc < size; ++proc) {
            int recv_start_row = proc * rows_per_proc + std::min(proc, remainder);
            int recv_end_row = recv_start_row + rows_per_proc + (proc < remainder ? 1 : 0);
            MPI_Recv(&C[recv_start_row][0], (recv_end_row - recv_start_row) * N, MPI_INT, proc, 0, MPI_COMM_WORLD, MPI_STATUS_IGNORE);
        }

        std::cout << "\nResult Matrix C (fragment 5x5):" << std::endl;
        printFragment(C, N);

        std::cout << "\nTime taken for matrix multiplication: " << max_elapsed << " seconds" << std::endl;
    } else {
        // Відправка результату з кожного процесу в процес 0
        MPI_Send(&C[start_row][0], (end_row - start_row) * N, MPI_INT, 0, 0, MPI_COMM_WORLD);
    }

    MPI_Finalize();
    return 0;
}

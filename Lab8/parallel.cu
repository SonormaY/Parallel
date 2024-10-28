#include <iostream>
#include <cuda_runtime.h>
#include <curand_kernel.h>
#include <chrono>


__global__ void generateMatrixCUDA(int *matrix, int n, unsigned long seed) {
    int row = blockIdx.y * blockDim.y + threadIdx.y;
    int col = blockIdx.x * blockDim.x + threadIdx.x;

    if (row < n && col < n) {
        curandState state;
        curand_init(seed, row * n + col, 0, &state);
        matrix[row * n + col] = curand(&state) % 100;
    }
}

__global__ void multiplyMatricesCUDA(int *matA, int *matB, int *result, int n) {
    int row = blockIdx.y * blockDim.y + threadIdx.y;
    int col = blockIdx.x * blockDim.x + threadIdx.x;

    if (row < n && col < n) {
        int sum = 0;
        for (int i = 0; i < n; ++i) {
            sum += matA[row * n + i] * matB[i * n + col];
        }
        result[row * n + col] = sum;
    }
}

void printMatrix(const int* matrix, int n) {
    for (int i = 0; i < n; ++i) {
        for (int j = 0; j < n; ++j) {
            std::cout << matrix[i * n + j] << " ";
        }
        std::cout << std::endl;
    }
}

int main() {
    int size;
    std::cout << "Enter the size of the square matrix: ";
    std::cin >> size;

    int *d_A, *d_B, *d_C;
    cudaMalloc(&d_A, size * size * sizeof(int));
    cudaMalloc(&d_B, size * size * sizeof(int));
    cudaMalloc(&d_C, size * size * sizeof(int));

    dim3 threadsPerBlock(16, 16);
    dim3 numBlocks((size + threadsPerBlock.x - 1) / threadsPerBlock.x, (size + threadsPerBlock.y - 1) / threadsPerBlock.y);

    unsigned long seed = time(0);
    generateMatrixCUDA<<<numBlocks, threadsPerBlock>>>(d_A, size, seed);
    generateMatrixCUDA<<<numBlocks, threadsPerBlock>>>(d_B, size, seed + 1);
    cudaDeviceSynchronize();

    int *h_A = new int[size * size];
    int *h_B = new int[size * size];
    cudaMemcpy(h_A, d_A, size * size * sizeof(int), cudaMemcpyDeviceToHost);
    cudaMemcpy(h_B, d_B, size * size * sizeof(int), cudaMemcpyDeviceToHost);

    if (size < 10)
    {
        std::cout << "Matrix A:" << std::endl;
        printMatrix(h_A, size);
        std::cout << "Matrix B:" << std::endl;
        printMatrix(h_B, size);
    }
    
    

    auto start = std::chrono::high_resolution_clock::now();
    multiplyMatricesCUDA<<<numBlocks, threadsPerBlock>>>(d_A, d_B, d_C, size);
    cudaDeviceSynchronize();
    auto stop = std::chrono::high_resolution_clock::now();
    std::chrono::duration<double> duration = stop - start;

    int *h_C = new int[size * size];
    cudaMemcpy(h_C, d_C, size * size * sizeof(int), cudaMemcpyDeviceToHost);

    if (size < 10)
    {
        std::cout << "Part of Result Matrix:" << std::endl;
        printMatrix(h_C, size);
    }
    

    std::cout << "Time taken for matrix multiplication (CUDA): " << duration.count() * 1000 << " ms" << std::endl;

    delete[] h_A;
    delete[] h_B;
    delete[] h_C;
    cudaFree(d_A);
    cudaFree(d_B);
    cudaFree(d_C);

    return 0;
}

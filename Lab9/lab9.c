#include <stdio.h>
#include <stdlib.h>
#include <pthread.h>
#include <sys/time.h>

#define MATRIX_SIZE 1000
#define NUM_THREADS 8

int A[MATRIX_SIZE][MATRIX_SIZE];
int B[MATRIX_SIZE][MATRIX_SIZE];
int C[MATRIX_SIZE][MATRIX_SIZE];

typedef struct {
    int startRow;
    int endRow;
} ThreadData;

void *multiplyMatrices(void *arg) {
    ThreadData *data = (ThreadData *)arg;
    int i, j, k;
    for (i = data->startRow; i < data->endRow; i++) {
        for (j = 0; j < MATRIX_SIZE; j++) {
            C[i][j] = 0;
            for (k = 0; k < MATRIX_SIZE; k++) {
                C[i][j] += A[i][k] * B[k][j];
            }
        }
    }
    pthread_exit(0);
}

int main() {
    int i, j;
    srand(time(NULL));

    for (i = 0; i < MATRIX_SIZE; i++) {
        for (j = 0; j < MATRIX_SIZE; j++) {
            A[i][j] = rand() % 900 + 100;  // Random 3-digit number between 100 and 999
            B[i][j] = rand() % 900 + 100;
        }
    }

    struct timeval start, end;
    gettimeofday(&start, NULL);

    pthread_t threads[NUM_THREADS];
    ThreadData threadData[NUM_THREADS];
    int rowsPerThread = MATRIX_SIZE / NUM_THREADS;

    for (i = 0; i < NUM_THREADS; i++) {
        threadData[i].startRow = i * rowsPerThread;
        threadData[i].endRow = (i == NUM_THREADS - 1) ? MATRIX_SIZE : (i + 1) * rowsPerThread;
        pthread_create(&threads[i], NULL, multiplyMatrices, (void *)&threadData[i]);
    }

    for (i = 0; i < NUM_THREADS; i++) {
        pthread_join(threads[i], NULL);
    }

    gettimeofday(&end, NULL);
    double elapsed = (end.tv_sec - start.tv_sec) + (end.tv_usec - start.tv_usec) / 1e6;
    printf("Matrix multiplication completed in %.5f seconds.\n", elapsed);

    printf("Top-left 5x5 corner of result matrix C:\n");
    for (i = 0; i < 5; i++) {
        for (j = 0; j < 5; j++) {
            printf("%d\t", C[i][j]);
        }
        printf("\n");
    }

    return 0;
}


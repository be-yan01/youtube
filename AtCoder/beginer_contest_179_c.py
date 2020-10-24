import math

N = int(input())
counter = 0

for A in range(1,N):
    counter += math.floor((N-1)/A)


print(counter)



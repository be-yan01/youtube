import math

N, K = map(int, input().split())

S = []
DP = [0] * (N + 1)

for i in range(K):
    L, R = map(int, input().split())
    S.extend(range(L, R + 1))

counter = 0

DP[1] = 1

for i in range(1,N):
    for d in S:
        if i + d <= N:
            #DP[i + d] += DP[i]
            DP[i + d] = (DP[i + d] + DP[i]) % 998244353

print(DP[N])

# def saiki(i):
#     global counter
#     if i > N:
#         return
#     if i == N:
#         counter +=1
#         return
#     for d in S:
#         saiki(i + d)

# saiki(1)

# print(counter)


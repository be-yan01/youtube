N = int(input())

counter = 0
flag = False

for i in range(N):
    vals = input().split()

    if vals[0] == vals[1]:
        counter += 1
        if counter == 3:
            flag = True
    else:
        counter = 0

if flag:
    print("Yes")
else:
    print("No")



list = []
for i in range(5):
    list.Add(i)
    print list
a = array(int, list)
for a_s in a:
    print a_s
a[2] += 5
print a
list[2] += 5
print list[2]
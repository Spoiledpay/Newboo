import System.IO
f = StreamReader("test.dat")
for line in f:
print line
f.Close()
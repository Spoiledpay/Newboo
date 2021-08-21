import System.IO

def copyFile(oldFile as string, newFile as
string):
f1 = StreamReader(oldFile)
f2 = StreamWriter(newFile)
for line in f1.ReadLine():
f2.WriteLine(line)
f1.Close()
f2.Close()
return
import System.IO
import System.Reflection

testfile = "newtestfile.txt"
try:
	if File.Exists(testfile):
		File.Delete(testfile)

	//"using" will dispose of (and close) the file stream for you
	using out = StreamWriter(testfile):
		out.WriteLine("  Some text for this file  ")
		out.WriteLine("# ignore this line")
		out.WriteLine("  Some more text  ")

	using input = StreamReader(testfile): //or you can use File.OpenText
		for line in input:
			line = line.Trim()
			if len(line) > 0 and line[0] != char('#'):
				print line

	//an example using enumerate and no "using"
	fileinput = File.OpenText(testfile)
	for index as int, line as string in enumerate(fileinput):
		print "line $index:", line.ToUpper()
	fileinput.Close()

except e:
	print "Error", e.ToString()
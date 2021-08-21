import System

try:
    print 1 / 0
except e as DivideByZeroException:
    print "Whoops"
print "Doing more..."
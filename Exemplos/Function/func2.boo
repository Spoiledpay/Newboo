def Hello():
    return "Hello, World!"

def Hello(name as string):
    return "Hello, ${name}!"

def Hello(num as int):
    return "Hello, Number ${num}!"

def Hello(name as string, other as string):
    return "Hello, ${name} and ${other}!"

print Hello()
print Hello("Monkey")
print Hello(2)
print Hello("Cat", "Dog")
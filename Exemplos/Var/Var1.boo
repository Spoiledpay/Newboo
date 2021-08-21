def Test(*args as (object)):
    return args.Length

print Test("hey", "there")
print Test(1, 2, 3, 4, 5)
print Test("test")

a = (5, 8, 1)
print Test(*a)
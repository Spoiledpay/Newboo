import System

class Cat:
    Name as string:
        get:
            return _name
        set:
            _name = value

    _name as string

fluffy = Cat()
fluffy.Name = 'Fluffy'

print fluffy.Name
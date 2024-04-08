using System;
// See https://projecteuler.net/problem=54
Console.WriteLine("Haiiii :3333");

Card a = new Card("");
Card b = new Card("");

int test = a.CompareTo(b);

class Hand : IComparable
{
    Card[] cards;

    public int CompareTo(object? obj)
    {
        if (obj is not Card)
            return -1;
        if (obj == this)
            return 0;


        throw new NotImplementedException();
    }
}

class Card : IComparable
{
    public int value;
    public Suite suite;
    
    public Card(string str)
    {

    }

    public int CompareTo(object? obj)
    {
        if (obj is not Card)
            throw new ArgumentException("Object was not of type 'Card'.");
        if (obj == this)
            return 0;
        return this.value.CompareTo(((Card)obj).value);
    }
}

enum Suite
{
    HEARTS,
    CLUBS,
    SPADES,
    DIAMONDS
}

/*
each card is represented by two chars:
V,S

V = card Value
S = card Suite
*/


/*
card Values:
2 = 2 
3 = 3
...
9 = 9
T = 10
J = 11
Q = 12
K = 13
A = 14
*/
using System;
using System.IO;
using System.Linq;
// See https://projecteuler.net/problem=54

string line;
StreamReader sr = new StreamReader("0054_poker.txt");
line = sr.ReadLine();

while (line != null)
{
    var split = line.Split(' ');
    var split1 = split.Take(new Range(0, 5));
    var split2 = split.Take(new Range(5, 10));

    var hand1 = new Card[5];
    var hand2 = new Card[5];
    int k = 0;
    foreach (var str in split1)
        hand1[k++] = new Card(str);
    k = 0;
    foreach (var str in split2)
        hand2[k++] = new Card(str);


    string h2 = "";
    foreach (var str in hand2)
        h2 += str;

    Console.WriteLine(h1 + "-" + h2);
    Console.WriteLine(line);
    line = sr.ReadLine();
}
sr.Close();
Console.ReadLine();









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
            throw new ArgumentException("Object wasn't a card :3");
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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
// See https://projecteuler.net/problem=54

string? line;
StreamReader sr = new StreamReader("0054_poker.txt");
line = sr.ReadLine();

int player1Wins = 0;
while (line != null)
{
    // split line into 2char elements
    var split = line.Split(' ');

    // split into two parts - one for each player
    var split1 = split.Take(new Range(0, 5));
    var split2 = split.Take(new Range(5, 10));

    // create card objects from 2 chars
    var hand1 = new Card[5];
    var hand2 = new Card[5];
    int k = 0;

    foreach (var str in split1)
        hand1[k++] = new Card(str);

    k = 0;
    foreach (var str in split2)
        hand2[k++] = new Card(str);

    // create hands and compare them
    if (new Hand(hand1).CompareTo(new Hand(hand2)) > 0)
        player1Wins++;

    // read the next line from the file
    line = sr.ReadLine();
}
sr.Close();
Console.WriteLine(player1Wins);
Console.ReadLine();


class Hand : IComparable
{
    Card[] cards;
    static Func<Card[], bool>[] checks = new Func<Card[], bool>[]
    {
        IsRoyalFlush,
        IsStraightFlush,
        IsFourOfAKind,
        IsFullHouse,
        IsFlush,
        IsStraight,
        IsThreeOfAKind,
        IsTwoPair,
        IsOnePair,
        IsHighCard,
    };

    public Hand(Card[] cards)
    {
        this.cards = cards;
        Array.Sort(this.cards);
    }

    public Rank HeighestRank(Card[] cards)
    {
        for (int i = 0; i < checks.Length; i++)
            if (checks[i](cards))
                return (Rank)i;
        throw new Exception("AAAAAAAAAHHHHHHHHHH");
    }

    private static bool IsTwoPair(Card[] cards)
    {
        var valueCounts = ValueCounts(cards);
        if (valueCounts.ContainsValue(2) && valueCounts.ContainsValue(3))
            return true;

        bool first = false;
        foreach(var pair in valueCounts)
        {
            if(pair.Value == 2)
            {
                if (first)
                    return true;
                first = true;
            }
        }
        return false;
    }

    private static bool IsFullHouse(Card[] cards)
    {
        var valueCounts = ValueCounts(cards);
        return valueCounts.ContainsValue(2) && valueCounts.ContainsValue(3);
    }

    private static Dictionary<int, int> ValueCounts(Card[] cards)
    {
        Dictionary<int, int> valueCounts = new();

        for (int i = 0; i < cards.Length; i++)
        {
            if (valueCounts.ContainsKey(cards[i].value))
                valueCounts[cards[i].value]++;
            else
                valueCounts.Add(cards[i].value, 1);
        }
        return valueCounts;
    }



    private static bool IsFourOfAKind(Card[] cards)
    {
        return IsNOfAKind(cards, 4);
    }

    private static bool IsThreeOfAKind(Card[] cards)
    {
        return IsNOfAKind(cards, 3);
    }

    private static bool IsOnePair(Card[] cards)
    {
        return IsNOfAKind(cards, 2);
    }
     
    private static bool IsNOfAKind(Card[] cards, int n)
    {
        int[] cardCounts = new int[14];
        foreach(var card in cards)
        {
            cardCounts[card.value]++;
            if (cardCounts[card.value] == n)
                return true;
        }
        return false;
    }

    private static bool IsHighCard(Card[] cards)
    {
        return true;
    }

    private static bool IsRoyalFlush(Card[] cards)
    {
        return IsStraightFlush(cards) && cards[4].value == 14;
    }

    private static bool IsStraightFlush(Card[] cards)
    {
        return IsFlush(cards) && IsStraight(cards);
    }

    private static bool IsFlush(Card[] cards)
    {
        var suite = cards[0].suite;
        for (int i = 1; i < cards.Length; i++)
            if (suite != cards[i].suite)
                return false;
        return true;
    }

    private static bool IsStraight(Card[] cards)
    {
        for (int i = 1; i < cards.Length; i++)
            if (cards[i].value != cards[i - 1].value + 1)
                return false;
        return true;
    }

    public int CompareTo(object? obj)
    {
        if (obj is not Hand)
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
        if (str.Count() != 2)
            throw new ArgumentException("`Card` class requires only 2 letters for the constructor");

        var ok = int.TryParse(str[0].ToString(), out value);

        if (!ok)
        {
            value = str[0] switch
            {
                'T' => 10,
                'J' => 11,
                'Q' => 12,
                'K' => 13,
                'A' => 14,
                _ => throw new Exception("AAAA")
            };
        }

        suite = str[1] switch
        {
            'H' => Suite.HEARTS,
            'C' => Suite.CLUBS,
            'S' => Suite.SPADES,
            'D' => Suite.DIAMONDS,
            _ => throw new Exception("EXPLODE")
        };
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

enum Rank
{
    HIGH_CARD       = 0,
    ONE_PAIR        = 1,
    TWO_PAIRS       = 2,
    THREE_OF_A_KIND = 3,
    STRAIGHT        = 4,
    FLUSH           = 5,
    FULL_HOUSE      = 6,
    FOUR_OF_A_KIND  = 7,
    STRAIGHT_FLUSH  = 8,
    ROYAL_FLUSH     = 9
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

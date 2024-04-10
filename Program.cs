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
    static Func<Card[], bool>[] checks =
    [
        IsRoyalFlush,
        IsStraightFlush,
        IsFourOfAKind,
        IsFullHouse,
        IsFlush,
        IsStraight,
        IsThreeOfAKind,
        IsTwoPair,
        IsOnePair,
    ];

    public Hand(Card[] cards)
    {
        this.cards = cards;
        Array.Sort(this.cards);
    }

    public int CompareTo(object? obj)
    {
        if (obj is not Hand)
            return -1;
        if (obj == this)
            return 0;

        var o = obj as Hand;

        var rank =  HeighestRank();
        var rankOther = o.HeighestRank();

        var comparison = ((int)rank).CompareTo((int)rankOther);
        if(comparison != 0) 
            return comparison;


        return rank switch
        {
            Rank.ROYAL_FLUSH        => throw new NotImplementedException(),
            Rank.STRAIGHT_FLUSH     => CompareHighCard(cards, o.cards),
            Rank.FOUR_OF_A_KIND     => CompareFourOfAKind(cards, o.cards),
            Rank.FULL_HOUSE         => CompareFullHouse(cards, o.cards),
            Rank.FLUSH              => CompareHighCard(cards, o.cards),
            Rank.STRAIGHT           => CompareHighCard(cards, o.cards),
            Rank.THREE_OF_A_KIND    => CompareThreeOfAKind(cards, o.cards),
            Rank.TWO_PAIRS          => CompareTwoPair(cards, o.cards),
            Rank.ONE_PAIR           => CompairOnePair(cards, o.cards),
            Rank.HIGH_CARD          => CompareHighCard(cards, o.cards),
            _ => throw new NotImplementedException()
        };
    }

    public static int CompairOnePair(Card[] handA, Card[] handB)
    {
        var valueCountsA = ValueCounts(handA);
        var valueCountsB = ValueCounts(handB);

        int pairA = -1;
        int pairB = -1;

        foreach (var pair in valueCountsA)
            if (pair.Value == 2)
                pairA = pair.Key;

        foreach (var pair in valueCountsB)
            if (pair.Value == 2)
                pairB = pair.Key;

        var pairComparison = pairA.CompareTo(pairB);
        if (pairComparison != 0)
            return pairComparison;
        return CompareHighCard(handA, handB);
    }

    public static int CompareTwoPair(Card[] handA, Card[] handB)
    {
        var valueCountsA = ValueCounts(handA);
        var valueCountsB = ValueCounts(handB);

        int highPairA = -1;
        int highPairB = -1;
        int lowPairA = -1;
        int lowPairB = -1;

        foreach (var pair in valueCountsA)
            if (pair.Value == 2)
            {
                if (highPairA == -1)
                    highPairA = pair.Key;
                else if(pair.Key > highPairA)
                {
                    lowPairA = highPairA;
                    highPairA = pair.Key;
                }
            }

        foreach (var pair in valueCountsB)
            if (pair.Value == 2)
            {
                if (highPairB == -1)
                    highPairB = pair.Key;
                else if (pair.Key > highPairB)
                {
                    lowPairB = highPairB;
                    highPairB = pair.Key;
                }
            }

        if (highPairA == -1 || highPairB == -1 || lowPairA == -1 || lowPairB == -1)
            throw new ArgumentException("nuuuuuuuuuuu :((((((");

        var highPairComparison = highPairA.CompareTo(highPairB);
        if (highPairComparison != 0)
            return highPairComparison;

        var lowPairComparison = lowPairA.CompareTo(lowPairB);
        if(lowPairComparison != 0)
            return lowPairComparison;

        return CompareHighCard(handA, handB);
    }

    public static int CompareThreeOfAKind(Card[] handA, Card[] handB)
    {
        var valueCountsA = ValueCounts(handA);
        var valueCountsB = ValueCounts(handB);

        int tripletA = -1;
        int tripletB = -1;

        foreach (var pair in valueCountsA)
            if (pair.Value == 3)
                tripletA = pair.Key;


        foreach (var pair in valueCountsB)
            if (pair.Value == 3)
                tripletB = pair.Key;

        if (tripletA == -1 || tripletB == -1)
            throw new ArgumentException("I want to cry");

        var tripletComparison = tripletA.CompareTo(tripletB);
        if (tripletComparison != 0)
            return tripletComparison;

        return CompareHighCard(handA, handB);
    }


    public static int CompareFullHouse(Card[] handA, Card[] handB)
    {
        var valueCountsA = ValueCounts(handA);
        var valueCountsB = ValueCounts(handB);

        int tripletA = -1;
        int tripletB = -1;
        int pairA = -1;
        int pairB = -1;

        foreach (var pair in valueCountsA)
            if (pair.Value == 3)
                tripletA = pair.Key;
            else if (pair.Value == 2)
                pairA = pair.Key;
            else
                throw new ArgumentException("waaahhh");

        foreach (var pair in valueCountsB)
            if (pair.Value == 3)
                tripletB = pair.Key;
            else if (pair.Value == 2)
                pairB = pair.Key;
            else
                throw new ArgumentException("waaahhh");

        if (tripletA == -1 || tripletB == -1 || pairA == -1 || pairB == -1)
            throw new ArgumentException("I am very sad");

        var tripletComparison = tripletA.CompareTo(tripletB);
        if(tripletComparison != 0)
            return tripletComparison;

        var pairComparison = pairA.CompareTo(pairB);
        if(pairComparison != 0) 
            return pairComparison;

        throw new ArgumentException("same??! can't be...");
    }

    public static int CompareFourOfAKind(Card[] handA, Card[] handB)
    {
        var valueCountsA = ValueCounts(handA);
        var valueCountsB = ValueCounts(handB);

        int valA = -1;
        foreach (var pair in valueCountsA)
            if (pair.Value == 4)
                valA = pair.Key;

        int valB = -1;
        foreach(var pair in valueCountsB)
            if(pair.Value == 4)
                valB = pair.Key;

        if (valA == -1 || valB == -1)
            throw new ArgumentException("somethin wrong :(((");

        int comparison = valA.CompareTo(valB);
        if (comparison != 0) return comparison;
        return CompareHighCard(handA, handB);
    }

    public static int CompareHighCard(Card[] handA, Card[] handB)
    {
        for(int i = handA.Length - 1; i >= 0; i--)
        {
            int val = handA[i].value.CompareTo(handB[i].value);
            if (val != 0)
                return val;
        }
        throw new ArgumentException("somethin weird happened, sowwy");
    }


    public Rank HeighestRank()
    {
        for (int i = 0; i < checks.Length; i++)
            if (checks[i](cards))
                return (Rank)(9-i);
        return Rank.HIGH_CARD;
    }

    private static bool IsTwoPair(Card[] cards)
    {
        var valueCounts = ValueCounts(cards);
        if (valueCounts.ContainsValue(2) && valueCounts.ContainsValue(3))
            return true;

        bool first = false;
        foreach(var pair in valueCounts)
        {
            if(pair.Value >= 2)
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
        var isTrue = valueCounts.ContainsValue(2) && valueCounts.ContainsValue(3);
        if (isTrue)
            return true;
        else
            return false;
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
        int[] cardCounts = new int[15];
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
        var isTrue = IsStraightFlush(cards) && cards[4].value == 14;
        if (isTrue)
            return true;
        else
            return false;
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
    ROYAL_FLUSH     = 9,
    STRAIGHT_FLUSH  = 8,
    FOUR_OF_A_KIND  = 7,
    FULL_HOUSE      = 6,
    FLUSH           = 5,
    STRAIGHT        = 4,
    THREE_OF_A_KIND = 3,
    TWO_PAIRS       = 2,
    ONE_PAIR        = 1,
    HIGH_CARD       = 0
}

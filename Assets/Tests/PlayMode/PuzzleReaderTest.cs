using System.Collections;
using JetBrains.Annotations;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PuzzleReaderTest
{
    PuzzleReader _reader;

    [SetUp]
    public void PuzzleReader_SetUp()
    {
        _reader = new PuzzleReader();
        
    }

    // A Test behaves as an ordinary method
    // Test the number of puzzle and their respective solution returned match the default (100 puzzle)
    [Test]
    public void PuzzleReader_ReadCSV_ReturnedPuzzleSolutionDefaultNumber_Test()
    {
        // Use the Assert class to test
        _reader.Load();

        Assert.AreEqual(100, _reader.Puzzle.Count);
        Assert.AreEqual(100, _reader.Solution.Count);
    }

    // Test the number of puzzle and their respective solution returned match the default (100 puzzle)
    [Test]
    [TestCase(10)]
    [TestCase(1000)]
    [TestCase(0)]
    [TestCase(1)]
    public void PuzzleReader_ReadCSV_ReturnedPuzzleSolutionNumber_Test(int numPuz)
    {
        // Use the Assert class to test
        _reader.Load(numPuz);

        Assert.AreEqual(numPuz, _reader.Puzzle.Count);
        Assert.AreEqual(numPuz, _reader.Solution.Count);
    }
}

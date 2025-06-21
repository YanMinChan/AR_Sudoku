using System.Collections;
using JetBrains.Annotations;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PuzzleReaderTest
{
    PuzzleReader _reader;
    string _filePath = "./Assets/Resources/sudoku.csv";

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
        _reader.ReadCSV(_filePath);

        Assert.AreEqual(100, _reader.Puzzle.Count);
        Assert.AreEqual(100, _reader.Solution.Count);
    }

    // Test the number of puzzle and their respective solution returned match the default (100 puzzle)
    // TODO: Add test of large number of puzzle later to see the speed of loading them
    [Test]
    [TestCase(10)]
    [TestCase(1000)]
    [TestCase(0)]
    [TestCase(1)]
    public void PuzzleReader_ReadCSV_ReturnedPuzzleSolutionNumber_Test(int numPuz)
    {
        // Use the Assert class to test
        _reader.ReadCSV(_filePath, numPuz);

        Assert.AreEqual(numPuz, _reader.Puzzle.Count);
        Assert.AreEqual(numPuz, _reader.Solution.Count);
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator NewTestScriptWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }
}

using NUnit.Framework;

// Real API calls share one free-tier quota; run tests sequentially.
[assembly: Parallelizable(ParallelScope.None)]

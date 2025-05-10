namespace WordleSolver.Interfaces
{
	internal interface IIndexable<T>
	{
		T this[int index] { get; }
		int Length { get; }
	}
}

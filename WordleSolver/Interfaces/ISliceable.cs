namespace WordleSolver.Interfaces
{
	internal interface ISliceable<T>
	{
		T[] this[Range range] { get; }
	}
}

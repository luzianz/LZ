namespace LZ {

	public interface IFactory<TObject, TParameters> {

		TObject CreateInstance(TParameters parameters);
	}
}

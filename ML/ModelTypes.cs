using Microsoft.ML.Data;

namespace ML
public class DocumentData
{
    public string Text { get; set; }
    public string Label { get; set; }
}

public class DocumentPrediction
{
    [ColumnName("PredictedLabel")]
    public string PredictedLabel { get; set; }
}
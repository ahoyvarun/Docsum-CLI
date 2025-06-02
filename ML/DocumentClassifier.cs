using Microsoft.ML;
using Microsoft.ML.Data;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ML
public static class DocumentClassifier
{
    private static readonly string modelPath = "documentClassificationModel.zip";

    private static readonly Lazy<PredictionEngine<DocumentData, DocumentPrediction>> predictionEngine = new(() =>
    {
        var mlContext = new MLContext();

        if (!File.Exists(modelPath))
        {
            Console.WriteLine("⚠️ Trained model not found. Defaulting to 'note'.");
            return null;
        }

        var trainedModel = mlContext.Model.Load(modelPath, out _);
        return mlContext.Model.CreatePredictionEngine<DocumentData, DocumentPrediction>(trainedModel);
    });

    public static Task<string> Classify(string text)
    {
        if (predictionEngine.Value == null)
            return Task.FromResult("note"); // fallback if model isn't trained yet

        var input = new DocumentData { Text = text };
        var prediction = predictionEngine.Value.Predict(input);
        return Task.FromResult(prediction.PredictedLabel);
    }
}
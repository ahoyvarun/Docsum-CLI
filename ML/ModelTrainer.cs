using Microsoft.ML;
using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.IO;

namespace ML
public static class ModelTrainer
{
    private static readonly string modelPath = "documentClassificationModel.zip";

    public static void TrainOrUpdateModel(string text, string label)
    {
        var mlContext = new MLContext();

        // Load existing model if available
        ITransformer trainedModel = null;
        DataViewSchema inputSchema = null;

        if (File.Exists(modelPath))
        {
            using var stream = new FileStream(modelPath, FileMode.Open, FileAccess.Read, FileShare.Read);
            trainedModel = mlContext.Model.Load(stream, out inputSchema);
        }

        // Prepare new training data
        var trainingData = new List<DocumentData>
        {
            new DocumentData { Text = text, Label = label }
        };

        var dataView = mlContext.Data.LoadFromEnumerable(trainingData);

        // Create or update pipeline
        var pipeline = mlContext.Transforms.Text.FeaturizeText("Features", nameof(DocumentData.Text))
            .Append(mlContext.Transforms.Conversion.MapValueToKey("Label"))
            .Append(mlContext.MulticlassClassification.Trainers.SdcaMaximumEntropy())
            .Append(mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"));

        ITransformer newModel = pipeline.Fit(dataView); // We'll replace this with true incremental logic later

        // Save the model
        using var fs = new FileStream(modelPath, FileMode.Create, FileAccess.Write, FileShare.Write);
        mlContext.Model.Save(newModel, dataView.Schema, fs);

        Console.WriteLine("✅ Model updated and saved.");
    }
}


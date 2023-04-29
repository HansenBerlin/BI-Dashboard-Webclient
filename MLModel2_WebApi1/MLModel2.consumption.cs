﻿// This file was auto-generated by ML.NET Model Builder.

using Microsoft.ML;
using Microsoft.ML.Data;

namespace MLModel2_WebApi1;

public partial class MLModel2
{
    /// <summary>
    /// model input class for MLModel2.
    /// </summary>
    #region model input class
    public class ModelInput
    {
        [LoadColumn(0)]
        [ColumnName(@"baseRent")]
        public float BaseRent { get; set; }

        [LoadColumn(1)]
        [ColumnName(@"livingSpace")]
        public float LivingSpace { get; set; }

        [LoadColumn(2)]
        [ColumnName(@"noRooms")]
        public float NoRooms { get; set; }

        [LoadColumn(3)]
        [ColumnName(@"condition")]
        public float Condition { get; set; }

        [LoadColumn(4)]
        [ColumnName(@"interiorQual")]
        public float InteriorQual { get; set; }

        [LoadColumn(5)]
        [ColumnName(@"lift")]
        public float Lift { get; set; }

        [LoadColumn(6)]
        [ColumnName(@"typeOfFlat")]
        public float TypeOfFlat { get; set; }

        [LoadColumn(7)]
        [ColumnName(@"telekomUploadSpeed")]
        public float TelekomUploadSpeed { get; set; }

        [LoadColumn(9)]
        [ColumnName(@"garden")]
        public float Garden { get; set; }

        [LoadColumn(10)]
        [ColumnName(@"heatingType")]
        public float HeatingType { get; set; }

        [LoadColumn(11)]
        [ColumnName(@"yearConstructed")]
        public float YearConstructed { get; set; }

        [LoadColumn(12)]
        [ColumnName(@"lon")]
        public float Lon { get; set; }

        [LoadColumn(13)]
        [ColumnName(@"lat")]
        public float Lat { get; set; }

    }

    #endregion

    /// <summary>
    /// model output class for MLModel2.
    /// </summary>
    #region model output class
    public class ModelOutput
    {
        [ColumnName(@"baseRent")]
        public float BaseRent { get; set; }

        [ColumnName(@"livingSpace")]
        public float LivingSpace { get; set; }

        [ColumnName(@"noRooms")]
        public float NoRooms { get; set; }

        [ColumnName(@"condition")]
        public float Condition { get; set; }

        [ColumnName(@"interiorQual")]
        public float InteriorQual { get; set; }

        [ColumnName(@"lift")]
        public float Lift { get; set; }

        [ColumnName(@"typeOfFlat")]
        public float TypeOfFlat { get; set; }

        [ColumnName(@"telekomUploadSpeed")]
        public float TelekomUploadSpeed { get; set; }

        [ColumnName(@"garden")]
        public float Garden { get; set; }

        [ColumnName(@"heatingType")]
        public float HeatingType { get; set; }

        [ColumnName(@"yearConstructed")]
        public float YearConstructed { get; set; }

        [ColumnName(@"lon")]
        public float Lon { get; set; }

        [ColumnName(@"lat")]
        public float Lat { get; set; }

        [ColumnName(@"Features")]
        public float[] Features { get; set; }

        [ColumnName(@"Score")]
        public float Score { get; set; }

    }

    #endregion

    private static string MLNetModelPath = Path.GetFullPath("MLModel2.mlnet");

    public static readonly Lazy<PredictionEngine<ModelInput, ModelOutput>> PredictEngine = new Lazy<PredictionEngine<ModelInput, ModelOutput>>(() => CreatePredictEngine(), true);


    private static PredictionEngine<ModelInput, ModelOutput> CreatePredictEngine()
    {
        var mlContext = new MLContext();
        ITransformer mlModel = mlContext.Model.Load(MLNetModelPath, out var _);
        return mlContext.Model.CreatePredictionEngine<ModelInput, ModelOutput>(mlModel);
    }

    /// <summary>
    /// Use this method to predict on <see cref="ModelInput"/>.
    /// </summary>
    /// <param name="input">model input.</param>
    /// <returns><seealso cref=" ModelOutput"/></returns>
    public static ModelOutput Predict(ModelInput input)
    {
        var predEngine = PredictEngine.Value;
        return predEngine.Predict(input);
    }

}
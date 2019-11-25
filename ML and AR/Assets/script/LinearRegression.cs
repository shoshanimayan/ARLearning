using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class LinearRegression : MonoBehaviour
{
    public TextAsset csvdata;
    public TextMeshProUGUI textPredictionResult;
    public TextMeshProUGUI textDatasetObject;
    public InputField inputObject;
    private List<double> yearValues = new List<double>();
    private List<double> quantityValues = new List<double>();


    public static void LinearRegressionCalc(
        double[] xValues,
        double[] yValues,
        out double yIntercept,
        out double slope)
    {
        if (xValues.Length != yValues.Length)
        {
            throw new Exception("Input values should be with the same length.");
        }

        double xSum = 0;
        double ySum = 0;
        double xSumSquared = 0;
        double ySumSquared = 0;
        double codeviatesSum = 0;

        for (var i = 0; i < xValues.Length; i++)
        {
            var x = xValues[i];
            var y = yValues[i];
            codeviatesSum += x * y;
            xSum += x;
            ySum += y;
            xSumSquared += x * x;
            ySumSquared += y * y;
        }

        var count = xValues.Length;
        var xSS = xSumSquared - ((xSum * xSum) / count);
        var ySS = ySumSquared - ((ySum * ySum) / count);

        var numeratorR = (count * codeviatesSum) - (xSum * ySum);
        var denomR = (count * xSumSquared - (xSum * xSum)) * (count * ySumSquared - (ySum * ySum));
        var coS = codeviatesSum - ((xSum * ySum) / count);

        var xMean = xSum / count;
        var yMean = ySum / count;

        yIntercept = yMean - ((coS / xSS) * xMean);
        slope = coS / xSS;
    }




    void Start()
    {
        Debug.Log(csvdata);

        string[] data = csvdata.text.Split(new char[] { '\n' });
        textDatasetObject.text = "Year Quantity";

        for (int i = 1; i < data.Length - 1; i++)
        {
            string[] row = data[i].Split(new char[] { ',' });

            if (row[1] != "")
            {
                DataInterface item = new DataInterface();

                int.TryParse(row[0], out item.year);
                int.TryParse(row[1], out item.quantity);

                yearValues.Add(item.year);
                quantityValues.Add(item.quantity);
                textDatasetObject.text += "\n" + item.year + " " + item.quantity;


            }
        }
    }

    public void PredictionTask()
    {

        double intercept, slope;
        LinearRegressionCalc(yearValues.ToArray(), quantityValues.ToArray(), out intercept, out slope);


        var predictedValue = (slope * int.Parse(inputObject.text)) + intercept;
        textPredictionResult.text = "Result: " + predictedValue;
        Debug.Log("Prediction for " + inputObject.text + " : " + predictedValue);

    }

}

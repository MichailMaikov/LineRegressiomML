using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplom
{
    static public class Linear
    {
        static public void LinearRegression(string[] args)
        {
            //Load sample data
            var sampleData = new MLModel1.ModelInput()
            {

            };

            //Load model and predict output
            var result = MLModel1.Predict(sampleData);
            //listBox1.Items.Add(result.Prediction);

        }
    }


}
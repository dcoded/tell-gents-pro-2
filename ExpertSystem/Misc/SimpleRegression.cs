
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.Statistics;

namespace ExpertSystem
{
    class SimpleRegression
    {
        private uint size_ = 0;

        private double[] x_;
        private double[] y_;

        Cache<double> pearson_   = new Cache<double>();
        Cache<double> slope_     = new Cache<double>();
        Cache<double> intercept_ = new Cache<double>();

        public SimpleRegression(int alloc = 128)
        {
            x_ = new double[alloc];
            y_ = new double[alloc];
        }

        public void AddData(double x, double y)
        {
            if (size_ >= x_.Length)
                Resize();

            x_[size_] = x;
            y_[size_] = y;

            size_++;

            pearson_.Bad();
            slope_.Bad();
            intercept_.Bad();
        }

        public void CreateModel()
        {
            if (!slope_ || !intercept_)
            {
                var mX = DenseMatrix.CreateFromColumns(new[]
                {
                    new DenseVector(x_.Length, 1),
                    new DenseVector(x_)
                });

                var mY = new DenseVector(y_);

                var p = mX.QR().Solve(mY);

                slope_ = p[0];
                intercept_ = p[1];
            }
        }

        public double Predict(double x)
        {
            if (!slope_)
            {
                CreateModel();
            }
            return (slope_ * x) + intercept_;
        }

        public double GetPearson()
        {
            if (!pearson_)
            {
                pearson_ = Correlation.Pearson(x_, y_);
            }

            return pearson_;
        }

        private void Resize()
        {
            double[] allocX = new double[x_.Length * 2];
            double[] allocY = new double[y_.Length * 2];

            for (int i = 0; i < x_.Length; i++)
            {
                allocX[i] = x_[i];
                allocY[i] = y_[i];
            }

            x_ = allocX;
            y_ = allocY;
        }
    }
}

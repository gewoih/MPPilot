const path = require('path');
const MiniCssExtractPlugin = require('mini-css-extract-plugin');
const TerserPlugin = require('terser-webpack-plugin');

module.exports = {
  entry: {
    script: './wwwroot/scripts/advertsPage.js',
  },
  output: {
    path: path.resolve(__dirname, './wwwroot/dist'),
    filename: 'mppilot.min.js',
  },
  module: {
    rules: [
      {
        test: /\.vue$/,
        loader: 'vue-loader',
      },
      {
        test: /\.js$/,
        loader: 'babel-loader',
        exclude: /node_modules/,
      },
      {
        test: /\.css$/,
        use: [MiniCssExtractPlugin.loader, 'css-loader'],
      },
    ],
  },
  resolve: {
    extensions: ['.js', '.vue']
  },
  plugins: [
    new MiniCssExtractPlugin({
      filename: 'mppilot.min.css',
    }),
  ],
  optimization: {
    minimizer: [new TerserPlugin()],
  },
  devtool: 'source-map',
};

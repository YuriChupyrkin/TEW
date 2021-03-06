const path = require('path');
const webpack = require('webpack');

module.exports = {
  entry: './app/ts/main.ts',
  module: {
    loaders: [
      {
        test: /\.ts$/,
        enforce: 'pre',
        loader: ['ts-loader', 'tslint-loader'],
      },
      {
        test: /\.scss$/,
        loader: ['style-loader', 'css-loader?sourceMap','sass-loader?sourceMap'],
      }
    ]
  },
  resolve: {
    extensions: ['.scss', '.tsx', '.ts', '.js' ]
  },
  // Add minification
  plugins: [
    new webpack.optimize.UglifyJsPlugin({
      sourceMap: true // dev environment
    })
  ],
  // Turn on sourcemaps
  devtool: 'source-map',
  output: {
    filename: 'bundle.js',
    path: path.resolve(__dirname, 'app/bundles')
  },
};
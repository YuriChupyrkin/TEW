const path = require('path');
const webpack = require('webpack');

module.exports = {
  entry: './app/ts/main.ts',
  module: {
    rules: [
      {
        test: /\.tsx?$/,
        use: 'ts-loader',
        exclude: /node_modules/
      }
    ]
  },
  resolve: {
    extensions: [ '.tsx', '.ts', '.js' ]
  },
  // Add minification
  plugins: [
    new webpack.optimize.UglifyJsPlugin()
  ],
  // Turn on sourcemaps
  devtool: 'source-map',
  output: {
    filename: 'bundle.js',
    path: path.resolve(__dirname, 'app/bundles')
  }
};
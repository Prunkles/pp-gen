const path = require('path');
const webpack = require('webpack');
const HtmlWebpackPlugin = require('html-webpack-plugin')
// const CopyWebpackPlugin = require('copy-webpack-plugin')


/*
 * SplitChunksPlugin is enabled by default and replaced
 * deprecated CommonsChunkPlugin. It automatically identifies modules which
 * should be splitted of chunk by heuristics using module duplication count and
 * module category (i. e. node_modules). And splits the chunks…
 *
 * It is safe to remove "splitChunks" from the generated configuration
 * and was added as an educational example.
 *
 * https://webpack.js.org/plugins/split-chunks-plugin/
 *
 */

/*
 * We've enabled MiniCssExtractPlugin for you. This allows your app to
 * use css modules that will be moved into a separate CSS file instead of inside
 * one of your module entries!
 *
 * https://github.com/webpack-contrib/mini-css-extract-plugin
 *
 */

const MiniCssExtractPlugin = require('mini-css-extract-plugin');


/*
 * We've enabled TerserPlugin for you! This minifies your app
 * in order to load faster and run less javascript.
 *
 * https://github.com/webpack-contrib/terser-webpack-plugin
 *
 */

const TerserPlugin = require('terser-webpack-plugin');


function resolve(filePath) {
    return path.isAbsolute(filePath) ? filePath : path.join(__dirname, filePath);
}

module.exports = {
    mode: 'development',
    entry: {
        webapp: resolve("build/Program.js"),
    },
    output: {
        path: resolve("dist/"),
        filename: "[name].js",
    },

    plugins: [
        new webpack.ProgressPlugin(),
        new MiniCssExtractPlugin({filename: 'main.[contenthash].css'}),
        new HtmlWebpackPlugin({
            filename: 'index.html',
            template: 'public/index.html',
        }),
    ],

    module: {
        rules: [{
            test: /.(less|css)$/,

            use: [{
                loader: MiniCssExtractPlugin.loader
            }, {
                loader: "style-loader"
            }, {
                loader: "css-loader",

                options: {
                    sourceMap: true
                }
            }, {
                loader: "less-loader",

                options: {
                    sourceMap: true
                }
            }]
        }]
    },

    optimization: {
        minimizer: [new TerserPlugin()],

        splitChunks: {
            cacheGroups: {
                vendors: {
                    priority: -10,
                    test: /[\\/]node_modules[\\/]/
                }
            },

            chunks: 'async',
            minChunks: 1,
            minSize: 30000,
            name: false
        }
    }
}
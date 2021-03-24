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

const isProduction = !process.argv.find(v => v.indexOf('webpack-dev-server') !== -1);

function resolve(filePath) {
    return path.isAbsolute(filePath) ? filePath : path.join(__dirname, filePath);
}

module.exports = {
    mode: 'development',
    entry: {
        webapp: [
            resolve("build/Program.js"),
            resolve("styles/main.css")
        ],
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
    
    resolve: {
        fallback: {
            path: false,
        },
    },
    
    devServer: {
        publicPath: '/',
        host: '0.0.0.0',
        port: 8080,
        proxy: {
            '/api/**': {
                target: 'http://localhost:' + '8085',
                changeOrigin: true,
                pathRewrite: {
                    '^/api': '',
                },
                ws: true,
            },
        },
    },
    

    module: {
        rules: [
            {
                test: /.(sass|scss|css)$/,
                use: [
                    isProduction
                        ? MiniCssExtractPlugin.loader
                        : "style-loader",
                    {
                        loader: "css-loader",
                        options: {
                            sourceMap: true
                        }
                    },
                    {
                        loader: 'sass-loader',
                        options: { implementation: require("sass") }
                    },
                ]
            }
        ]
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
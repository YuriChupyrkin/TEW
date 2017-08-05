var ts = require('gulp-typescript');
var gulp = require('gulp');
var clean = require('gulp-clean');
var sass   = require('gulp-sass');

var destPath = './libs/';
var tslint = require("gulp-tslint");

var tsProject = ts.createProject('tsconfig.json', {
    typescript: require('typescript')
});

var pathForFiles = [
    "app/ts/*.ts",
    "app/ts/services/*.ts",
    "app/ts/models/*.ts",
    "app/ts/helpers/*.ts",
    "app/ts/components/*.ts",
    "app/ts/helpComponents/*.ts",
    "app/ts/components/*/*.ts"
];

// Delete the dist directory
gulp.task('clean', function () {
    return gulp.src(destPath)
        .pipe(clean());
});

gulp.task('css', function() {
  return gulp.src('app/scss/*/*.scss')
    .pipe(sass())
    .pipe(gulp.dest('./app/css'));
});

gulp.task('ts', function (done) {
    var tsResult = gulp.src(pathForFiles)
        .pipe(ts(tsProject), undefined, ts.reporter.fullReporter());

    return tsResult.js.pipe(gulp.dest('./app/js'));
});

gulp.task('tslint', function (done) {
    var tsResult = gulp.src(pathForFiles)
        .pipe(tslint({
            formatter: "verbose"
        }))
        .pipe(tslint.report());
});

gulp.task('ts_tslint', function (done) {
    var tsResult = gulp.src(pathForFiles)
        .pipe(tslint({
            formatter: "verbose"
        }))
        .pipe(tslint.report())
        .pipe(ts(tsProject), undefined, ts.reporter.fullReporter())

    return tsResult.js.pipe(gulp.dest('./app/js'));
});

gulp.task('build', ['css', 'ts']);
gulp.task('default', ['ts_tslint', 'css']);
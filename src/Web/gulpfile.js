/// <binding BeforeBuild='copy-assets, min' Clean='clean' />
var gulp = require('gulp'),
    rimraf = require("rimraf"),
    concat = require("gulp-concat"),
    cleanCSS = require("gulp-clean-css"),
    uglify = require("gulp-uglify"),
    lodash = require('lodash'),
    pump = require('pump');

var paths = {
    webroot: "./wwwroot/",
    dependencies: "./node_modules/"
};

paths.js = paths.webroot + "js/**/*.js";
paths.minJs = paths.webroot + "js/**/*.min.js";
paths.css = paths.webroot + "css/**/*.css";
paths.minCss = paths.webroot + "css/**/*.min.css";
paths.concatJsDest = paths.webroot + "js/site.min.js";
paths.concatCssDest = paths.webroot + "css/site.min.css";

gulp.task('copy-assets', gulp.series(function (done) {
    var assets = {
        js:
            [
                paths.dependencies + 'bootstrap/dist/js/bootstrap.js',
                paths.dependencies + 'jquery/dist/jquery.js',
                paths.dependencies + 'jquery-validation/dist/*.js',
                paths.dependencies + 'jquery-validation-unobtrusive/dist/jquery.validate.unobtrusive.js',
                paths.dependencies + 'popper.js/dist/umd/popper.js',
                paths.dependencies + 'normalize-css/normalize.js',
                paths.dependencies + 'bootstrap/js/dist/alert.js',
                paths.dependencies + 'bootstrap/js/dist/carousel.js',
                paths.dependencies + 'knockout/build/output/knockout-latest.debug.js',
                paths.dependencies + 'knockout/build/output/knockout-latest.js',
                paths.dependencies + 'knockout-paging/dist/knockout-paging.js',
                paths.dependencies + 'knockout-paging/dist/knockout-paging.min.js',
                paths.dependencies + 'flatpickr/dist/flatpickr.js'
            ],
        css:
            [
                paths.dependencies + 'bootstrap/dist/css/bootstrap.css',
                paths.dependencies + 'normalize-css/normalize.css',
                paths.dependencies + 'font-awesome/css/font-awesome.css',
                paths.dependencies + 'flatpickr/dist/flatpickr.css'
            ],
        fonts:
            [
                paths.dependencies + 'font-awesome/fonts/**.*'
            ]
    };
    lodash(assets).forEach(function (assets, type) {
        gulp.src(assets).pipe(gulp.dest(paths.webroot + type));
    })

    done();
}));

gulp.task("clean:js", function (cb) {
    rimraf(paths.concatJsDest, cb);
});

gulp.task("clean:css", function (cb) {
    rimraf(paths.concatCssDest, cb);
});

gulp.task("min:js", gulp.series(function (done) {
    gulp.src([paths.js, "!" + paths.minJs], { base: "." })
        .pipe(concat(paths.concatJsDest))
        .pipe(uglify())
        .pipe(gulp.dest("."))
    done();
}));

gulp.task("min:css", gulp.series(function (done) {
    gulp.src([paths.css, "!" + paths.minCss])
        .pipe(concat(paths.concatCssDest))
        .pipe(cleanCSS({ compatibility: 'ie8' }))
        .pipe(gulp.dest("."))
    done();
}));

gulp.task("clean", gulp.series("clean:js", "clean:css"));

gulp.task("min", gulp.series("min:js", "min:css"));
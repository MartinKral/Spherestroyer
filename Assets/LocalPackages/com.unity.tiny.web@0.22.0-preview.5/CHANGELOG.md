# Changelog
All notable changes to this package will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/)
and this project adheres to [Semantic Versioning](http://semver.org/spec/v2.0.0.html).

## [0.22.0] - 2020-02-05

* Update package dependencies
* Fix a performance issue where a storm of repeated attempts to unlock the AudioContext would cause Firefox and Chrome to hang.
* Fix old browsers not necessarily having `AudioContext.resume()` or `AudioContext.suspend()` functionality.

* Properly initialize canvas size based on screen size and devicePixelRatio.  DisplayInfo.screenDpiScale can
be used to control the ratio of render resolution and display resolution.

## [0.21.0] - 2020-01-21

* Update package dependencies

## [0.20.0-preview.2] - 2019-12-10

* Update the package to use Unity '2019.3.0f1' or later
* This is the first release of Tiny for web support

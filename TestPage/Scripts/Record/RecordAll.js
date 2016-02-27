(function () {
    var params = {},
        r = /([^&=]+)=?([^&]*)/g;

    function d(s) {
        return decodeURIComponent(s.replace(/\+/g, ' '));
    }

    var match, search = window.location.search;
    while (match = r.exec(search.substring(1))) {
        params[d(match[1])] = d(match[2]);

        if (d(match[2]) === 'true' || d(match[2]) === 'false') {
            params[d(match[1])] = d(match[2]) === 'true' ? true : false;
        }
    }

    window.params = params;
})();


        function intallFirefoxScreenCapturingExtension() {
            InstallTrigger.install({
                'Foo': {
                    // URL: 'https://addons.mozilla.org/en-US/firefox/addon/enable-screen-capturing/',
                    URL: 'https://addons.mozilla.org/firefox/downloads/file/355418/enable_screen_capturing_in_firefox-1.0.006-fx.xpi?src=cb-dl-hotness',
                    toString: function () {
                        return this.URL;
                    }
                }
            });
        }

var recordingDIV = document.querySelector('.recordrtc');
var recordingMedia = recordingDIV.querySelector('.recording-media');
var recordingPlayer = recordingDIV.querySelector('video');
var mediaContainerFormat = recordingDIV.querySelector('.media-container-format');

window.onbeforeunload = function () {
    recordingDIV.querySelector('button').disabled = false;
    recordingMedia.disabled = false;
    mediaContainerFormat.disabled = false;
};

recordingDIV.querySelector('button').onclick = function () {
    var button = this;

    if (button.innerHTML === 'Dừng quay') {
        button.disabled = true;
        button.disableStateWaiting = true;
        setTimeout(function () {
            button.disabled = false;
            button.disableStateWaiting = false;
        }, 2 * 1000);

        button.innerHTML = 'Bắt đầu quay màn hình';

        function stopStream() {
            if (button.stream && button.stream.stop) {
                button.stream.stop();
                button.stream = null;
            }
        }

        if (button.recordRTC) {
            if (button.recordRTC.length) {
                button.recordRTC[0].stopRecording(function (url) {
                    if (!button.recordRTC[1]) {
                        button.recordingEndedCallback(url);
                        stopStream();

                        saveToDiskOrOpenNewTab(button.recordRTC[0]);
                        return;
                    }

                    button.recordRTC[1].stopRecording(function (url) {
                        button.recordingEndedCallback(url);
                        stopStream();
                    });
                });
            }
            else {
                button.recordRTC.stopRecording(function (url) {
                    button.recordingEndedCallback(url);
                    stopStream();

                    saveToDiskOrOpenNewTab(button.recordRTC);
                });
            }
        }

        return;
    }

    button.disabled = true;

    var commonConfig = {
        onMediaCaptured: function (stream) {
            button.stream = stream;
            if (button.mediaCapturedCallback) {
                button.mediaCapturedCallback();
            }

            button.innerHTML = 'Dừng quay';
            button.disabled = false;
        },
        onMediaStopped: function () {
            button.innerHTML = 'Bắt đầu quay màn hình';
           
            if (!button.disableStateWaiting) {
                button.disabled = true;
            }
        },
        onMediaCapturingFailed: function (error) {
            if (error.name === 'PermissionDeniedError' && !!navigator.mozGetUserMedia) {
                intallFirefoxScreenCapturingExtension();
            }

            commonConfig.onMediaStopped();
        }
    };

    var mimeType = 'video/webm';
    if (mediaContainerFormat.value === 'Mp4') {
        mimeType = 'video/mp4';
    }

    if (recordingMedia.value === 'record-video') {
        captureVideo(commonConfig);

        button.mediaCapturedCallback = function () {
            button.recordRTC = RecordRTC(button.stream, {
                type: mediaContainerFormat.value === 'Gif' ? 'gif' : 'video',
                recorderType: isChrome && mediaContainerFormat.value !== 'Gif' && typeof MediaRecorder !== 'undefined' ? MediaStreamRecorder : null,
                mimeType: isChrome ? null : mimeType,
                disableLogs: params.disableLogs || false,
                canvas: {
                    width: params.canvas_width || 1280,
                    height: params.canvas_height || 720
                },
                frameInterval: typeof params.frameInterval !== 'undefined' ? parseInt(params.frameInterval) : 20 // minimum time between pushing frames to Whammy (in milliseconds)
            });

            button.recordingEndedCallback = function (url) {
                recordingPlayer.src = null;
                recordingPlayer.srcObject = null;

                if (mediaContainerFormat.value === 'Gif') {
                    recordingPlayer.pause();
                    recordingPlayer.poster = url;

                    recordingPlayer.onended = function () {
                        recordingPlayer.pause();
                        recordingPlayer.poster = URL.createObjectURL(button.recordRTC.blob);
                    };
                    return;
                }

                recordingPlayer.src = url;
                recordingPlayer.play();

                recordingPlayer.onended = function () {
                    recordingPlayer.pause();
                    recordingPlayer.src = URL.createObjectURL(button.recordRTC.blob);
                };
            };

            button.recordRTC.startRecording();
        };
    }

    if (recordingMedia.value === 'record-audio') {
        captureAudio(commonConfig);

        button.mediaCapturedCallback = function () {
            button.recordRTC = RecordRTC(button.stream, {
                type: 'audio',
                mimeType: mimeType,
                bufferSize: typeof params.bufferSize == 'undefined' ? 0 : parseInt(params.bufferSize),
                sampleRate: typeof params.sampleRate == 'undefined' ? 44100 : parseInt(params.sampleRate),
                leftChannel: params.leftChannel || false,
                disableLogs: params.disableLogs || false,
                recorderType: webrtcDetectedBrowser === 'edge' ? StereoAudioRecorder : null
            });

            button.recordingEndedCallback = function (url) {
                var audio = new Audio();
                audio.src = url;
                audio.controls = true;
                recordingPlayer.parentNode.appendChild(document.createElement('hr'));
                recordingPlayer.parentNode.appendChild(audio);

                if (audio.paused) audio.play();

                audio.onended = function () {
                    audio.pause();
                    audio.src = URL.createObjectURL(button.recordRTC.blob);
                };
            };

            button.recordRTC.startRecording();
        };
    }

    if (recordingMedia.value === 'record-audio-plus-video') {
        captureAudioPlusVideo(commonConfig);

        button.mediaCapturedCallback = function () {

            if (webrtcDetectedBrowser !== 'firefox') { // opera or chrome etc.
                button.recordRTC = [];

                if (!params.bufferSize) {
                    // it fixes audio issues whilst recording 720p
                    params.bufferSize = 16384;
                }

                var audioRecorder = RecordRTC(button.stream, {
                    type: 'audio',
                    bufferSize: typeof params.bufferSize == 'undefined' ? 0 : parseInt(params.bufferSize),
                    sampleRate: typeof params.sampleRate == 'undefined' ? 44100 : parseInt(params.sampleRate),
                    leftChannel: params.leftChannel || false,
                    disableLogs: params.disableLogs || false,
                    recorderType: webrtcDetectedBrowser === 'edge' ? StereoAudioRecorder : null
                });

                var videoRecorder = RecordRTC(button.stream, {
                    type: 'video',
                    // recorderType: isChrome && typeof MediaRecorder !== 'undefined' ? MediaStreamRecorder : null,
                    disableLogs: params.disableLogs || false,
                    canvas: {
                        width: params.canvas_width || 320,
                        height: params.canvas_height || 240
                    },
                    frameInterval: typeof params.frameInterval !== 'undefined' ? parseInt(params.frameInterval) : 20 // minimum time between pushing frames to Whammy (in milliseconds)
                });

                // to sync audio/video playbacks in browser!
                videoRecorder.initRecorder(function () { //It is a function that can be used to initiate recorder however skip getting recording outputs. It will provide maximum accuracy in the outputs after using startRecording method.
                    audioRecorder.initRecorder(function () {
                        audioRecorder.startRecording();
                        videoRecorder.startRecording();
                    });
                });

                button.recordRTC.push(audioRecorder, videoRecorder);

                button.recordingEndedCallback = function () {
                    var audio = new Audio();
                    audio.src = audioRecorder.toURL();
                    audio.controls = true;
                    audio.autoplay = true;

                    audio.onloadedmetadata = function () {
                        recordingPlayer.src = videoRecorder.toURL();
                        recordingPlayer.play();
                    };

                    recordingPlayer.parentNode.appendChild(document.createElement('hr'));
                    recordingPlayer.parentNode.appendChild(audio);

                    if (audio.paused) audio.play();
                };
                return;
            }

            button.recordRTC = RecordRTC(button.stream, {
                type: 'video',
                mimeType: mimeType,
                disableLogs: params.disableLogs || false,
                // we can't pass bitrates or framerates here
                // Firefox MediaRecorder API lakes these features
            });

            button.recordingEndedCallback = function (url) {
                recordingPlayer.srcObject = null;
                recordingPlayer.muted = false;
                recordingPlayer.src = url;
                recordingPlayer.play();

                recordingPlayer.onended = function () {
                    recordingPlayer.pause();
                    recordingPlayer.src = URL.createObjectURL(button.recordRTC.blob);
                };
            };

            button.recordRTC.startRecording();
        };
    }

    if (recordingMedia.value === 'record-screen') {
        captureScreen(commonConfig);

        button.mediaCapturedCallback = function () {
            button.recordRTC = RecordRTC(button.stream, {
                type: mediaContainerFormat.value === 'Gif' ? 'gif' : 'video',
                mimeType: mimeType,
                recorderType: isChrome && mediaContainerFormat.value !== 'Gif' && typeof MediaRecorder !== 'undefined' ? MediaStreamRecorder : null,
                disableLogs: params.disableLogs || false,
                canvas: {
                    width: params.canvas_width || 1280,
                    height: params.canvas_height || 720
                }
            });

            button.recordingEndedCallback = function (url) {
                recordingPlayer.src = null;
                recordingPlayer.srcObject = null;

                if (mediaContainerFormat.value === 'Gif') {
                    recordingPlayer.pause();
                    recordingPlayer.poster = url;
                    recordingPlayer.onended = function () {
                        recordingPlayer.pause();
                        recordingPlayer.poster = URL.createObjectURL(button.recordRTC.blob);
                    };
                    return;
                }

                recordingPlayer.src = url;
                recordingPlayer.play();
            };

            button.recordRTC.startRecording();
        };
    }

    if (recordingMedia.value === 'record-audio-plus-screen') {
        captureAudioPlusScreen(commonConfig);

        button.mediaCapturedCallback = function () {
            button.recordRTC = RecordRTC(button.stream, {
                type: 'video',
                mimeType: mimeType,
                disableLogs: params.disableLogs || false,
                // we can't pass bitrates or framerates here
                // Firefox MediaRecorder API lakes these features
            });

            button.recordingEndedCallback = function (url) {
                recordingPlayer.srcObject = null;
                recordingPlayer.muted = false;
                recordingPlayer.src = url;
                recordingPlayer.play();

                recordingPlayer.onended = function () {
                    recordingPlayer.pause();
                    recordingPlayer.src = URL.createObjectURL(button.recordRTC.blob);
                };
            };

            button.recordRTC.startRecording();
        };
    }
};

function captureVideo(config) {
    captureUserMedia({ video: true }, function (videoStream) {
        recordingPlayer.srcObject = videoStream;
        recordingPlayer.play();

        config.onMediaCaptured(videoStream);

        videoStream.onended = function () {
            config.onMediaStopped();
        };
    }, function (error) {
        config.onMediaCapturingFailed(error);
    });
}

function captureAudio(config) {
    captureUserMedia({ audio: true }, function (audioStream) {
        recordingPlayer.srcObject = audioStream;
        recordingPlayer.play();

        config.onMediaCaptured(audioStream);

        audioStream.onended = function () {
            config.onMediaStopped();
        };
    }, function (error) {
        config.onMediaCapturingFailed(error);
    });
}

function captureAudioPlusVideo(config) {
    captureUserMedia({ video: true, audio: true }, function (audioVideoStream) {
        recordingPlayer.srcObject = audioVideoStream;
        recordingPlayer.play();

        config.onMediaCaptured(audioVideoStream);

        audioVideoStream.onended = function () {
            config.onMediaStopped();
        };
    }, function (error) {
        config.onMediaCapturingFailed(error);
    });
}

function captureScreen(config) {
    getScreenId(function (error, sourceId, screenConstraints) {
        if (error === 'not-installed') {
            document.write('<h1><a target="_blank" href="https://chrome.google.com/webstore/detail/screen-capturing/ajhifddimkapgcifgcodmmfdlknahffk">Bạn chưa cài tiện ích để quay màn hình. Hãy nhấn vào đây để cài sau đó tải lại trang.</a></h1>');
        }

        if (error === 'permission-denied') {
            alert('Yêu cầu quay màn hình bị từ chối.');
        }

        if (error === 'installed-disabled') {
            alert('Hãy bật tiện ích mở rộng trước khi quay.');
        }

        if (error) {
            config.onMediaCapturingFailed(error);
            return;
        }

        captureUserMedia(screenConstraints, function (screenStream) {
            recordingPlayer.srcObject = screenStream;
            recordingPlayer.play();

            config.onMediaCaptured(screenStream);

            screenStream.onended = function () {
                config.onMediaStopped();
            };
        }, function (error) {
            config.onMediaCapturingFailed(error);
        });
    });
}

function captureAudioPlusScreen(config) {
    getScreenId(function (error, sourceId, screenConstraints) {
        if (error === 'not-installed') {
            document.write('<h1><a target="_blank" href="https://chrome.google.com/webstore/detail/screen-capturing/ajhifddimkapgcifgcodmmfdlknahffk">Please install this chrome extension then reload the page.</a></h1>');
        }

        if (error === 'permission-denied') {
            alert('Screen capturing permission is denied.');
        }

        if (error === 'installed-disabled') {
            alert('Please enable chrome screen capturing extension.');
        }

        if (error) {
            config.onMediaCapturingFailed(error);
            return;
        }

        screenConstraints.audio = true;

        captureUserMedia(screenConstraints, function (screenStream) {
            recordingPlayer.srcObject = screenStream;
            recordingPlayer.play();

            config.onMediaCaptured(screenStream);

            screenStream.onended = function () {
                config.onMediaStopped();
            };
        }, function (error) {
            config.onMediaCapturingFailed(error);
        });
    });
}

function captureUserMedia(mediaConstraints, successCallback, errorCallback) {
    navigator.mediaDevices.getUserMedia(mediaConstraints).then(successCallback).catch(errorCallback);
}

function setMediaContainerFormat(arrayOfOptionsSupported) {
    var options = Array.prototype.slice.call(
        mediaContainerFormat.querySelectorAll('option')
    );

    var selectedItem;
    options.forEach(function (option) {
        option.disabled = true;

        if (arrayOfOptionsSupported.indexOf(option.value) !== -1) {
            option.disabled = false;

            if (!selectedItem) {
                option.selected = true;
                selectedItem = option;
            }
        }
    });
}

recordingMedia.onchange = function () {
    var options = [];
    if (webrtcDetectedBrowser === 'firefox') {
        if (this.value === 'record-audio') {
            options.push('Ogg');
        }
        else {
            options.push('WebM', 'Mp4');
        }

        setMediaContainerFormat(options);
        return;
    }
    if (this.value === 'record-audio') {
        setMediaContainerFormat(['WAV', 'Ogg']);
        return;
    }
    setMediaContainerFormat(['WebM', 'Mp4', 'Ogg']);
};

if (webrtcDetectedBrowser === 'edge') {
    // webp isn't supported in Microsoft Edge
    // neither MediaRecorder API
    // so lets disable both video/screen recording options

    console.warn('Neither MediaRecorder API nor webp is supported in Microsoft Edge. You cam merely record audio.');

    recordingMedia.innerHTML = '<option value="record-audio">Audio</option>';
    setMediaContainerFormat(['WAV']);
}

//if (webrtcDetectedBrowser === 'firefox') {
//    // Firefox implemented both MediaRecorder API as well as WebAudio API
//   //  Their MediaRecorder implementation supports both audio/video recording in single container format
//    // Remember, we can't currently pass bit-rates or frame-rates values over MediaRecorder API (their implementation lakes these features)

//    recordingMedia.innerHTML = '<option value="record-audio-plus-video">Audio+Video</option>'
//                                + '<option value="record-audio-plus-screen">Audio+Screen</option>'
//                                + recordingMedia.innerHTML;

//    setMediaContainerFormat(['WebM', 'Mp4']);
//}

//if (webrtcDetectedBrowser === 'chrome') {
//    recordingMedia.innerHTML = '<option value="record-audio-plus-video">Audio+Video</option>'
//                                + recordingMedia.innerHTML;
//    console.info('This RecordRTC demo merely tries to playback recorded audio/video sync inside the browser. It still generates two separate files (WAV/WebM).');
//}

//Bắt đầu thêm

function PostBlob(blob, fileType, fileName) {
    // FormData
    var formData = new FormData();
    formData.append(fileType + '-filename', fileName);
    formData.append(fileType + '-blob', blob);

    // progress-bar
    var hr = document.createElement('hr');
    container.appendChild(hr);
    var strong = document.createElement('strong');
    strong.id = 'percentage';
    strong.innerHTML = fileType + ' upload progress: ';
    container.appendChild(strong);
    var progress = document.createElement('progress');
    container.appendChild(progress);

    // POST the Blob using XHR2
    xhr('/Screen/PostRecordedAudioVideo', formData, progress, percentage, function (fName) {
        container.appendChild(document.createElement('hr'));
        var mediaElement = document.createElement(fileType);

        var source = document.createElement('source');
        source.src = location.href + 'uploads/' + fName.replace(/"/g, '');

        if (fileType == 'video') source.type = 'video/webm; codecs="vp8, vorbis"';
        if (fileType == 'audio') source.type = !!navigator.mozGetUserMedia ? 'audio/ogg' : 'audio/wav';

        mediaElement.appendChild(source);

        mediaElement.controls = true;
        container.appendChild(mediaElement);
        mediaElement.play();

        progress.parentNode.removeChild(progress);
        strong.parentNode.removeChild(strong);
        hr.parentNode.removeChild(hr);
    });
}

var stop = document.getElementById('stop')
stop.onclick = function () {
    fileName = 'testFile';

    if (!isFirefox) {
        recordAudio.stopRecording(function () {
            PostBlob(recordAudio.getBlob(), 'audio', fileName + '.wav');
        });
    } else {
        recordAudio.stopRecording(function (url) {
            preview.src = url;
            PostBlob(recordAudio.getBlob(), 'video', fileName + '.webm');
        });
    }

    if (!isFirefox) {
        recordingPlayer.stopRecording(function () {
            PostBlob(recordingPlayer.getBlob(), 'video', fileName + '.webm');
        });
    }

};

function xhr(url, data, progress, percentage, callback) {
    var request = new XMLHttpRequest();
    request.onreadystatechange = function () {
        if (request.readyState == 4 && request.status == 200) {
            callback(request.responseText);
        }
    };

    if (url.indexOf('/Screen/DeleteFile') == -1) {
        request.upload.onloadstart = function () {
            percentage.innerHTML = 'Upload started...';
        };

        request.upload.onprogress = function (event) {
            progress.max = event.total;
            progress.value = event.loaded;
            percentage.innerHTML = 'Upload Progress ' + Math.round(event.loaded / event.total * 100) + "%";
        };

        request.upload.onload = function () {
            percentage.innerHTML = 'Saved!';
        };
    }

    request.open('POST', url);
    request.send(data);
}

//Hết thêm

function saveToDiskOrOpenNewTab(recordRTC) {
    recordingDIV.querySelector('#save-to-disk').parentNode.style.display = 'block';
    recordingDIV.querySelector('#save-to-disk').onclick = function () {
        if (!recordRTC) return alert('No recording found.');
        recordRTC.save();
    };

    recordingDIV.querySelector('#open-new-tab').onclick = function () {
        if (!recordRTC) return alert('No recording found.');

        window.open(recordRTC.toURL());


    };
}

          // todo: need to check exact chrome browser because opera also uses chromium framework
          var isChrome = !!navigator.webkitGetUserMedia;

// DetectRTC.js - https://github.com/muaz-khan/WebRTC-Experiment/tree/master/DetectRTC
// Below code is taken from RTCMultiConnection-v1.8.js (http://www.rtcmulticonnection.org/changes-log/#v1.8)
var DetectRTC = {};

(function () {

    var screenCallback;

    DetectRTC.screen = {
        chromeMediaSource: 'screen',
        getSourceId: function (callback) {
            if (!callback) throw '"callback" parameter is mandatory.';
            screenCallback = callback;
            window.postMessage('get-sourceId', '*');
        },
        isChromeExtensionAvailable: function (callback) {
            if (!callback) return;

            if (DetectRTC.screen.chromeMediaSource == 'desktop') return callback(true);

            // ask extension if it is available
            window.postMessage('are-you-there', '*');

            setTimeout(function () {
                if (DetectRTC.screen.chromeMediaSource == 'screen') {
                    callback(false);
                }
                else callback(true);
            }, 2000);
        },
        onMessageCallback: function (data) {
            if (!(typeof data == 'string' || !!data.sourceId)) return;

            console.log('chrome message', data);

            // "cancel" button is clicked
            if (data == 'PermissionDeniedError') {
                DetectRTC.screen.chromeMediaSource = 'PermissionDeniedError';
                if (screenCallback) return screenCallback('PermissionDeniedError');
                else throw new Error('PermissionDeniedError');
            }

            // extension notified his presence
            if (data == 'rtcmulticonnection-extension-loaded') {
                if (document.getElementById('install-button')) {
                    document.getElementById('install-button').parentNode.innerHTML = '<strong>Tuyệt vời!</strong> <a href="https://chrome.google.com/webstore/detail/screen-capturing/ajhifddimkapgcifgcodmmfdlknahffk" target="_blank">Tiện ích mở rộng của Google chrome</a> đã đc cài đặt.';
                }
                DetectRTC.screen.chromeMediaSource = 'desktop';
            }

            // extension shared temp sourceId
            if (data.sourceId) {
                DetectRTC.screen.sourceId = data.sourceId;
                if (screenCallback) screenCallback(DetectRTC.screen.sourceId);
            }
        },
        getChromeExtensionStatus: function (callback) {
            if (!!navigator.mozGetUserMedia) return callback('not-chrome');

            var extensionid = 'ajhifddimkapgcifgcodmmfdlknahffk';

            var image = document.createElement('img');
            image.src = 'chrome-extension://' + extensionid + '/icon.png';
            image.onload = function () {
                DetectRTC.screen.chromeMediaSource = 'screen';
                window.postMessage('are-you-there', '*');
                setTimeout(function () {
                    if (!DetectRTC.screen.notInstalled) {
                        callback('installed-enabled');
                    }
                }, 2000);
            };
            image.onerror = function () {
                DetectRTC.screen.notInstalled = true;
                callback('not-installed');
            };
        }
    };

    // check if desktop-capture extension installed.
    if (window.postMessage && isChrome) {
        DetectRTC.screen.isChromeExtensionAvailable();
    }
})();

DetectRTC.screen.getChromeExtensionStatus(function (status) {
    if (status == 'installed-enabled') {
        if (document.getElementById('install-button')) {
            document.getElementById('install-button').parentNode.innerHTML = '<strong>Tuyệt vời!</strong> <a href="https://chrome.google.com/webstore/detail/screen-capturing/ajhifddimkapgcifgcodmmfdlknahffk" target="_blank">Tiện ích mở rộng của Google chrome</a> đã đc cài đặt.';
        }
        DetectRTC.screen.chromeMediaSource = 'desktop';
    }
});

window.addEventListener('message', function (event) {
    if (event.origin != window.location.origin) {
        return;
    }

    DetectRTC.screen.onMessageCallback(event.data);
});
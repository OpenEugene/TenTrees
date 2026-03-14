/* Module Script */
var OpenEug = OpenEug || {};

OpenEug.TenTrees = OpenEug.TenTrees || {};

OpenEug.TenTrees.Enrollment = {
    SignaturePad: (function () {
        const pads = {};

        function init(canvasId) {
            const canvas = document.getElementById(canvasId);
            if (!canvas) return;

            const ctx = canvas.getContext('2d');
            let drawing = false;
            let strokes = [];
            let currentStroke = [];

            function fitCanvas() {
                const parent = canvas.parentElement;
                canvas.width = parent ? parent.clientWidth : 300;
                canvas.height = 180;
                redraw();
            }

            function renderPlaceholder() {
                ctx.fillStyle = '#aaa';
                ctx.font = '18px sans-serif';
                ctx.textAlign = 'center';
                ctx.textBaseline = 'middle';
                ctx.fillText('Sign here', canvas.width / 2, canvas.height / 2);
            }

            function redraw() {
                ctx.clearRect(0, 0, canvas.width, canvas.height);
                if (strokes.length === 0) { renderPlaceholder(); return; }
                ctx.strokeStyle = '#111';
                ctx.lineWidth = 2.5;
                ctx.lineCap = 'round';
                ctx.lineJoin = 'round';
                for (var i = 0; i < strokes.length; i++) {
                    var stroke = strokes[i];
                    if (stroke.length < 2) continue;
                    ctx.beginPath();
                    ctx.moveTo(stroke[0].x, stroke[0].y);
                    for (var j = 1; j < stroke.length; j++) ctx.lineTo(stroke[j].x, stroke[j].y);
                    ctx.stroke();
                }
            }

            function getPoint(e) {
                const rect = canvas.getBoundingClientRect();
                const src = e.touches ? e.touches[0] : e;
                return { x: src.clientX - rect.left, y: src.clientY - rect.top };
            }

            function onStart(e) {
                drawing = true;
                if (strokes.length === 0) ctx.clearRect(0, 0, canvas.width, canvas.height);
                currentStroke = [getPoint(e)];
                e.preventDefault();
            }

            function onMove(e) {
                if (!drawing) return;
                const p = getPoint(e);
                currentStroke.push(p);
                if (currentStroke.length >= 2) {
                    const prev = currentStroke[currentStroke.length - 2];
                    ctx.strokeStyle = '#111';
                    ctx.lineWidth = 2.5;
                    ctx.lineCap = 'round';
                    ctx.lineJoin = 'round';
                    ctx.beginPath();
                    ctx.moveTo(prev.x, prev.y);
                    ctx.lineTo(p.x, p.y);
                    ctx.stroke();
                }
                e.preventDefault();
            }

            function onEnd(e) {
                if (drawing && currentStroke.length > 0) {
                    strokes.push(currentStroke.slice());
                    currentStroke = [];
                }
                drawing = false;
                e.preventDefault();
            }

            canvas.addEventListener('mousedown', onStart);
            canvas.addEventListener('mousemove', onMove);
            canvas.addEventListener('mouseup', onEnd);
            canvas.addEventListener('mouseleave', onEnd);
            canvas.addEventListener('touchstart', onStart, { passive: false });
            canvas.addEventListener('touchmove', onMove, { passive: false });
            canvas.addEventListener('touchend', onEnd, { passive: false });

            fitCanvas();

            pads[canvasId] = {
                canvas: canvas,
                isBlank: function () { return strokes.length === 0; },
                clear: function () {
                    strokes = [];
                    currentStroke = [];
                    ctx.clearRect(0, 0, canvas.width, canvas.height);
                    renderPlaceholder();
                },
                getSvg: function () {
                    if (strokes.length === 0) return null;
                    const w = canvas.width;
                    const h = canvas.height;
                    var paths = '';
                    for (var i = 0; i < strokes.length; i++) {
                        var s = strokes[i];
                        if (s.length < 2) continue;
                        var d = '';
                        for (var j = 0; j < s.length; j++) {
                            d += (j === 0 ? 'M' : 'L') + s[j].x.toFixed(1) + ',' + s[j].y.toFixed(1) + ' ';
                        }
                        paths += '<path d="' + d.trim() + '" fill="none" stroke="#111" stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round"/>';
                    }
                    return '<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 ' + w + ' ' + h + '" style="width:100%;height:auto;display:block;">' + paths + '</svg>';
                }
            };
        }

        function clear(canvasId) {
            if (pads[canvasId]) pads[canvasId].clear();
        }

        function isBlank(canvasId) {
            return pads[canvasId] ? pads[canvasId].isBlank() : true;
        }

        function getData(canvasId) {
            return pads[canvasId] ? pads[canvasId].getSvg() : null;
        }

        return { init: init, clear: clear, isBlank: isBlank, getData: getData };
    })()
};

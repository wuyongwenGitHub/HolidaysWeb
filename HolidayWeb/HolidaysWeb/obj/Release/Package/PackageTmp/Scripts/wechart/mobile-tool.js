// JavaScript Document
(function(n){
var e=n.document,
	t=e.documentElement,
	i=640,
	o="orientationchange"in n?"orientationchange":"resize",
    UA = navigator.userAgent,
	isAndroid = /android|adr/gi.test(UA),
	isIos = /iphone|ipod|ipad/gi.test(UA) && !isAndroid, 
	maxwidth = e.documentElement.dataset.mw || 640, 
	minwidth = e.documentElement.dataset.mw || 320, 
	a=function(){		
		dpr = isIos? Math.min(window.devicePixelRatio, 3) : 1,
        t.dataset.dpr =dpr;
		var n=t.clientWidth;
			if(n>maxwidth){
				n = maxwidth;
			}else if(n<minwidth){
				n = minwidth;
			}			
			if (n / dpr > maxwidth) {
				n = maxwidth * dpr;
			}
		t.style.fontSize=n/16+"px"
	};
    e.addEventListener && (n.addEventListener(o, a, !1), e.addEventListener("DOMContentLoaded", a, !1));
})(window);
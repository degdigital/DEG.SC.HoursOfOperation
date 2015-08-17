(function (window, document) {

	var hoursWrapperClass = 'hours-wrapper',
		hoursSelectClass = 'hours-select',
		closedSelectClass = 'closed-select',
		openAllHoursheckboxClass = 'openallhours-checkbox',
		hoursWrapperEl,
		rawInputEls,
		hoursSelectEls,
		closedSelectEls,
		timeArray = [
			"12:00 AM",
			"12:30 AM",
			"1:00 AM",
			"1:30 AM",
			"2:00 AM",
			"2:30 AM",
			"3:00 AM",
			"3:30 AM",
			"4:00 AM",
			"4:30 AM",
			"5:00 AM",
			"5:30 AM",
			"6:00 AM",
			"6:30 AM",
			"7:00 AM",
			"7:30 AM",
			"8:00 AM",
			"8:30 AM",
			"9:00 AM",
			"9:30 AM",
			"10:00 AM",
			"10:30 AM",
			"11:00 AM",
			"11:30 AM",
			"12:00 PM",
			"12:30 PM",
			"1:00 PM",
			"1:30 PM",
			"2:00 PM",
			"2:30 PM",
			"3:00 PM",
			"3:30 PM",
			"4:00 PM",
			"4:30 PM",
			"5:00 PM",
			"5:30 PM",
			"6:00 PM",
			"6:30 PM",
			"7:00 PM",
			"7:30 PM",
			"8:00 PM",
			"8:30 PM",
			"9:00 PM",
			"9:30 PM",
			"10:00 PM",
			"10:30 PM",
			"11:00 PM",
			"11:30 PM"
		];

	function init() {
		hoursWrapperEl = document.querySelector('.' + hoursWrapperClass);
		hoursSelectEls = Array.prototype.slice.call(hoursWrapperEl.querySelectorAll('.' + hoursSelectClass));
		closedSelectEls = Array.prototype.slice.call(hoursWrapperEl.querySelectorAll('.' + closedSelectClass));
		setInitialValues();
		bindEvents();
	};

	function setInitialValues() {
		hoursSelectEls.forEach(createHoursDropdown);
		rawInputEls = Array.prototype.slice.call(hoursWrapperEl.querySelectorAll('.raw-inputs input'));
		for (var i = 0; i < rawInputEls.length; i++) {
			var el = rawInputEls[i];
			var elValue = el.value;
			if (elValue.length > 0) {
				var elTargetAttr = el.getAttribute('id'),
					elTarget = hoursWrapperEl.querySelector('[data-target="' + elTargetAttr + '"]');
				if ((elTarget.getAttribute('type') === 'checkbox') && (elValue === '1')) {
					if (elTarget.classList.contains(openAllHoursheckboxClass)) {
						[hoursSelectEls,closedSelectEls].forEach(function(els) {
							els.forEach(disableEl);
						});
					}
					elTarget.setAttribute('checked', true);
				} else {
					elTarget.value = elValue;
					if ((elTarget.classList.contains(closedSelectClass)) && (elTarget.value === '1')) {
						var siblingHoursSelectEls = getSiblingSelectEls(elTarget, hoursSelectClass);
						siblingHoursSelectEls.forEach(disableEl);
					}
				}
				
			}
		}
	};

	function createHoursDropdown(selectEl) {
		timeArray.forEach(function(time) {
			var optionEl = document.createElement('option');
			optionEl.textContent = time;
			optionEl.setAttribute('value', time);
			selectEl.appendChild(optionEl);
		});
	};

	function bindEvents() {
		document.addEventListener('change', function(e) {
			var targetEl = e.target;
			if (isDescendentByClass(hoursSelectClass, targetEl) !== false) {
				setTargetEl(targetEl);
			} else if (isDescendentByClass(closedSelectClass, targetEl) !== false) {
				setTargetEl(targetEl);
				var siblingHoursSelectEls = getSiblingSelectEls(targetEl, hoursSelectClass);
				if (targetEl.value === '0') {
					siblingHoursSelectEls.forEach(enableEl);
				} else {
					siblingHoursSelectEls.forEach(disableEl);
				}
			} else if (isDescendentByClass(openAllHoursheckboxClass, targetEl) !== false) {
				if (targetEl.checked) {
					setTargetEl(targetEl, '1');
					[hoursSelectEls,closedSelectEls].forEach(function(els) {
						els.forEach(disableEl);
					});
				} else {
					setTargetEl(targetEl, '0');
					closedSelectEls.forEach(function(closedEl) {
						enableEl(closedEl);
						if (closedEl.value === '0') {
							var siblingHoursSelectEls = getSiblingSelectEls(closedEl, hoursSelectClass);
							siblingHoursSelectEls.forEach(enableEl);
						}
					});
				}
			}
		})
	};

	function setTargetEl(el, paramVal) {
		var targetSelector = el.getAttribute('data-target');
		if (targetSelector !== null) {
			var target = hoursWrapperEl.querySelector('#' + targetSelector);
			if (target) {
				if (typeof paramVal !== 'undefined') {
					target.value = paramVal;
				} else {
					target.value = el.value;
				}
			}
		}
	};

	function disableEl(el) {
		el.setAttribute('disabled', true);
	};

	function enableEl(el) {
		el.removeAttribute('disabled');
	};

	function getSiblingSelectEls(el, siblingClass) {
		var parent = el.parentNode,
			rowWrapper = parent.parentNode;
		return Array.prototype.slice.call(rowWrapper.querySelectorAll('.' + siblingClass));
	};

	function isDescendentByClass(parentClass, el) {
        if (el.classList.contains(parentClass)) {
            return el;
        }
        var node = el.parentNode;
        while (node != null) {
            if ((typeof node.classList !== 'undefined') && (node.classList.contains(parentClass))) {
                return node;
            }
            node = node.parentNode;
        }
        return false;
    };
	
	document.addEventListener('DOMContentLoaded', function() {
		init();
	});

})(window, document);
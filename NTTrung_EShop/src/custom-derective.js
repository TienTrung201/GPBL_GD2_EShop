/**
 * Author: Tiến Trung (11/07/2023)
 * Description: Custom Directives tooltip
 * Muốn dùng chỉ cần v-tooltip="Nội dung"
 */
export const tooltipDerective = (el, binding) => {
    try {
        const text = binding.value;
        let tooltip = null;
        tooltip = document.createElement('div');
        tooltip.classList.add('tooltip');
        const showTooltip = (e) => {
            // tạo element tooltip
            tooltip = document.createElement('div');
            tooltip.classList.add('tooltip');
            tooltip.textContent = text;
            // trường hợp tooltip muốn absolute theo cha dành cho trường hợp muốn hủy tooltip trên một overley
            if (binding.modifiers.absoluteTop) {
                tooltip.classList.add('tooltip-absolute--top');
                el.appendChild(tooltip);
                el.style.position = 'relative';
            } else if (binding.modifiers.absoluteBottom) {
                tooltip.classList.add('tooltip-absolute--bottom');
                el.appendChild(tooltip);
                el.style.position = 'relative';
            }
            // trường hợp tooltip muốn top nhưng để position fixed theo cha
            else if (binding.modifiers.top) {
                document.querySelector('#app').appendChild(tooltip);
                // debugger;
                tooltip.classList.add('tooltip-top');
                const element = e.target.getBoundingClientRect();
                const left = element.right - element.width / 2;
                const bottom = element.top - tooltip.clientHeight - 4;
                tooltip.style.left = left + 'px';
                tooltip.style.top = bottom + 'px';
            } else {
                // trường hợp tooltip auto theo chuột
                const left = e.clientX;
                const bottom = e.clientY;
                tooltip.style.left = left + 'px';
                tooltip.style.top = bottom + 'px';
                document.querySelector('#app').appendChild(tooltip);
            }
        };

        const hideTooltip = () => {
            if (tooltip) {
                tooltip.remove();
                tooltip = null;
            }
        };
        if (!text) {
            el.onmouseenter = hideTooltip;
        } else {
            el.onmouseenter = showTooltip;
            el.addEventListener('mouseleave', hideTooltip);
        }
    } catch (error) {
        console.log(error);
    }

    // el.onmouseenter = showTooltip;
    // el.addEventListener('mouseleave', hideTooltip);
};

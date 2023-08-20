<template>
    <div class="wrapper-loading center">
        <div>
            <svg
                xmlns="http://www.w3.org/2000/svg"
                xmlns:xlink="http://www.w3.org/1999/xlink"
                width="32"
                height="32"
                viewBox="0 0 32 32"
            >
                <title>32px_dots 06</title>
                <g :class="'nc-loop_dots-06-32'" :transform="'rotate(' + rotation + ' 16 16)'">
                    <circle fill="#2ca01c" cx="16" cy="3" r="3" />
                    <circle fill="#2ca01c" cx="25.19239" cy="6.80761" r="3" :opacity="opacity" />
                    <circle fill="#2ca01c" cx="29" cy="16" r="3" :opacity="opacity" />
                    <circle fill="#2ca01c" cx="25.19239" cy="25.19239" r="3" :opacity="opacity" />
                    <circle fill="#2ca01c" cx="16" cy="29" r="3" :opacity="opacity" />
                    <circle fill="#2ca01c" cx="6.80761" cy="25.19239" r="3" :opacity="opacity" />
                    <circle fill="#2ca01c" cx="3" cy="16" r="3" :opacity="opacity + 0.2" />
                    <circle fill="#2ca01c" cx="6.80761" cy="6.80761" r="3" :opacity="opacity + 0.4" />
                </g>
            </svg>
        </div>
    </div>
</template>

<script>
//Icon loading của cty tách svg ra đặt vào script
//Hàm bên dưới là để cho icon có thể quay
export default {
    data() {
        return {
            rotation: 0,
            opacity: 0.4,
            animationId: null,
            start: null,
        };
    },
    mounted() {
        this.initAnimation();
        document.addEventListener('visibilitychange', this.handleVisibilityChange);
    },
    beforeUnmount() {
        this.resetAnimation();
        document.removeEventListener('visibilitychange', this.handleVisibilityChange);
    },
    methods: {
        initAnimation() {
            this.animationId = window.requestAnimationFrame(this.triggerAnimation);
        },
        resetAnimation() {
            window.cancelAnimationFrame(this.animationId);
        },
        triggerAnimation(timestamp) {
            if (!this.start) {
                this.start = timestamp;
            }
            const elapsed = timestamp - this.start;
            if (elapsed > 800) {
                this.start = this.start + 800;
            }
            this.rotation = (parseInt(Math.min(elapsed / 100, 8)) % 8) * 45;
            if (document.documentElement.contains(this.$el)) {
                window.requestAnimationFrame(this.triggerAnimation);
            }
        },
        handleVisibilityChange() {
            if (document.visibilityState === 'hidden') {
                this.resetAnimation();
            } else {
                this.initAnimation();
            }
        },
    },
};
</script>

<style lang="scss" scoped>
.wrapper-loading {
    position: fixed;
    top: 0;
    right: 0;
    left: 0;
    bottom: 0;
    transform: translateX(0);
    z-index: 1;
}
/* Your component-specific styles can go here */
</style>

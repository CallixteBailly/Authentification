<script setup lang="ts">
import { toRef } from 'vue';
import { useField } from 'vee-validate';

const props = defineProps({
    type: {
        type: String,
        default: 'text',
    },
    value: {
        type: String,
        default: '',
    },
    name: {
        type: String,
        required: true,
    },
    label: {
        type: String,
        required: true,
    },
    successMessage: {
        type: String,
        default: '',
    },
    placeholder: {
        type: String,
        default: '',
    },
});

// use `toRef` to create reactive references to `name` prop which is passed to `useField`
// this is important because vee-validte needs to know if the field name changes
// https://vee-validate.logaretm.com/v4/guide/composition-api/caveats
const name = toRef(props, 'name');

// we don't provide any rules here because we are using form-level validation
// https://vee-validate.logaretm.com/v4/guide/validation#form-level-validation
const {
    value: inputValue,
    errorMessage,
    handleBlur,
    handleChange,
    meta,
} = useField(name, undefined, {
    initialValue: props.value,
});
</script>

<template>
    <div class="TextInput" :class="{ 'has-error': !!errorMessage, success: meta.valid }">
        <label :for="name">{{ label }}</label>
        <input :name="name" :id="name" :type="type" :value="inputValue" :placeholder="placeholder" @input="handleChange"
            @blur="handleBlur" />
        <p class="help-message" v-show="errorMessage || meta.valid">
            {{ errorMessage || successMessage }}
        </p>
    </div>
</template>

<style scoped>
.TextInput {
    position: relative;
    margin: 20px 0 0;
    width: 310px;
    border-bottom: 2px solid #ffffff;
}

.TextInput input {
    width: 100%;
    background: transparent;
    border: none;
    outline: none;
    font-size: 1em;
    padding: 0 35px 0 5px;
    color: #fff;
}

input:-webkit-autofill,
input:-webkit-autofill:focus,
select:-webkit-autofill:focus {
    -webkit-text-fill-color: #fff;
    -webkit-box-shadow: 0 0 0px 1000px transparent inset;
    transition: background-color 5000s ease-in-out 0s;
}

.TextInput label {
    position: relative;
    top: 55%;
    color: #ffffff;
    font-size: 1em;
    pointer-events: none;
}


.help-message {
    position: absolute;
    bottom: calc(-1.5 * 1em);
    left: 0;
    margin: 0;
    font-size: 14px;
}

/* .TextInput.has-error  {
    background-color: var(--error-bg-color);
    color: var(--error-color);
} */

/* .TextInput.has-error input:focus {
    border-color: var(--error-color);
} */

.TextInput.has-error .help-message {
    color: var(--error-color);
}

/* .TextInput.success input {
    background-color: var(--success-bg-color);
    color: var(--success-color);
} */

/* .TextInput.success input:focus {
    border-color: var(--success-color);
} */

.TextInput.success .help-message {
    color: var(--success-color);
}
</style>

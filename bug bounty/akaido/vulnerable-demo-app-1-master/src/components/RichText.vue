<template>
    <section class="rich-text">
      <div class="content" v-text="html"></div>
    </section>
  </template>
  
  <script>
  export default {
    name: "RichText",
    props: {
      html: {
        type: String,
        required: true,
      },
    },
  };
  </script>
  
  <style scoped>
  .rich-text .content { line-height: 1.6; }
  </style>
  
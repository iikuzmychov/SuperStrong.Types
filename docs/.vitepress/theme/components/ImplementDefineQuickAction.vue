<script setup lang="ts">
import QuickActionMockup from './QuickActionMockup.vue'
</script>

<template>
  <QuickActionMockup
    primitive="int"
    class-name="Age"
    :menu="[
      { label: 'Generate constructor from members...' },
      { label: `Generate constructor 'Age()'` },
      { label: 'Extract interface...' },
      { label: 'Extract base class...' },
      { label: 'Implement Define()', selected: true },
      { label: 'Implement Define() explicitly' },
      { label: 'Override Equals(T) and GetHashCode()' },
      { label: 'Override ToString()' },
    ]"
  >
    <div class="line indent-4 added"><span class="plus">+</span><span class="keyword">public static partial</span> <span class="type">StrongTypeDefinition</span>&lt;<span class="keyword">int</span>&gt; <span class="method">Define</span>() => <span class="type">StrongType</span>.<span class="method">Define</span>&lt;<span class="keyword">int</span>&gt;();</div>
  </QuickActionMockup>
</template>

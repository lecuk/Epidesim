#version 330 core

in vec2 outPosition;
in vec4 outColor;
in float outRadius;

float makeCircle(vec2 position, float radius){
  return step(radius, length(position - outPosition));
}

void main(){
  vec2 position = gl_FragCoord.xy;
  float alpha = makeCircle(position, outRadius);
  gl_FragColor = vec4(outColor.xyz, alpha);
}
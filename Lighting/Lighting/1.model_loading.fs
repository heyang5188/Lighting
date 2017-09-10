#version 330 core
out vec4 FragColor;

in VS_OUT {
    vec3 FragPos;
    vec2 TexCoords;
    vec3 TangentLightPos;
    vec3 TangentViewPos;
    vec3 TangentFragPos;
} fs_in;

struct Light {
  
  
    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
	
    float constant;
    float linear;
    float quadratic;
};

uniform sampler2D texture_diffuse1;
uniform sampler2D texture_specular1;
uniform sampler2D texture_normal1;
uniform Light light;




void main()
{  
// Obtain normal from normal map in range [0,1]
    vec3 normal = texture(texture_normal1, fs_in.TexCoords).rgb;
    // Transform normal vector to range [-1,1]
	vec3 norm = normalize(normal * 2.0 - 1.0); 
	//ambient
	vec3 ambient = light.ambient * texture(texture_diffuse1, fs_in.TexCoords).rgb;

	//diffuse
	//vec3 norm = normalize(Normal);
	vec3 lightDir = normalize(fs_in.TangentLightPos - fs_in.TangentFragPos);
	float diff = max(dot(norm,lightDir),0.0);
	vec3 diffuse = diff * light.diffuse * texture(texture_diffuse1, fs_in.TexCoords).rgb;

	//specular
	vec3 viewDir = normalize(fs_in.TangentViewPos - fs_in.TangentFragPos);
    vec3 reflectDir = reflect(-lightDir, norm);
   float spec = pow(max(dot(viewDir, reflectDir), 0.0), 32);
	//vec3 halfwayDir = normalize(lightDir + viewDir);  
   // float spec = pow(max(dot(normal, halfwayDir), 0.0), 32.0);

    vec3 specular = light.specular * spec * texture(texture_specular1,  fs_in.TexCoords).rgb;  

	  // attenuation
   // float distance    = length(light.position - FragPos);
   // float attenuation = 1.0 / (light.constant + light.linear * distance + light.quadratic * (distance * distance));    

	//  ambient  *= attenuation;  
    //diffuse   *= attenuation;
    //specular *= attenuation; 

	 vec3 result = ambient + diffuse + specular;
   FragColor = vec4(result, 1.0);
}
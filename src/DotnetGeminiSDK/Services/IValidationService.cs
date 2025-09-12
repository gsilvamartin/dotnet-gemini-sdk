using System;
using System.Collections.Generic;
using System.Linq;
using DotnetGeminiSDK.Model;

namespace DotnetGeminiSDK.Services
{
    /// <summary>
    /// Interface for validation service
    /// </summary>
    public interface IValidationService
    {
        /// <summary>
        /// Validate if a model is supported for text generation
        /// </summary>
        /// <param name="modelName">Model name to validate</param>
        /// <returns>True if model is supported for text generation</returns>
        bool IsValidTextModel(string modelName);
        
        /// <summary>
        /// Validate if a model is supported for multimodal processing
        /// </summary>
        /// <param name="modelName">Model name to validate</param>
        /// <returns>True if model is supported for multimodal processing</returns>
        bool IsValidMultimodalModel(string modelName);
        
        /// <summary>
        /// Validate if a model is supported for embeddings
        /// </summary>
        /// <param name="modelName">Model name to validate</param>
        /// <returns>True if model is supported for embeddings</returns>
        bool IsValidEmbeddingModel(string modelName);
        
        /// <summary>
        /// Validate function declaration parameters
        /// </summary>
        /// <param name="functionDeclaration">Function declaration to validate</param>
        /// <returns>Validation result</returns>
        ValidationResult ValidateFunctionDeclaration(Model.Request.FunctionDeclaration functionDeclaration);
        
        /// <summary>
        /// Validate system instruction
        /// </summary>
        /// <param name="systemInstruction">System instruction to validate</param>
        /// <returns>Validation result</returns>
        ValidationResult ValidateSystemInstruction(string systemInstruction);
        
        /// <summary>
        /// Validate generation config
        /// </summary>
        /// <param name="generationConfig">Generation config to validate</param>
        /// <returns>Validation result</returns>
        ValidationResult ValidateGenerationConfig(Model.Request.GenerationConfig generationConfig);
        
        /// <summary>
        /// Validate safety settings
        /// </summary>
        /// <param name="safetySettings">Safety settings to validate</param>
        /// <returns>Validation result</returns>
        ValidationResult ValidateSafetySettings(IEnumerable<Model.Request.SafetySetting> safetySettings);
    }
    
    /// <summary>
    /// Default validation service implementation
    /// </summary>
    public class ValidationService : IValidationService
    {
        private readonly ILoggingService _loggingService;
        
        /// <summary>
        /// Initialize validation service
        /// </summary>
        /// <param name="loggingService">Logging service</param>
        public ValidationService(ILoggingService loggingService)
        {
            _loggingService = loggingService;
        }
        
        /// <inheritdoc/>
        public bool IsValidTextModel(string modelName)
        {
            if (string.IsNullOrWhiteSpace(modelName))
                return false;
                
            var isValid = GeminiModels.TextModels.Contains(modelName);
            if (!isValid)
            {
                _loggingService.LogWarning($"Invalid text model: {modelName}");
            }
            
            return isValid;
        }
        
        /// <inheritdoc/>
        public bool IsValidMultimodalModel(string modelName)
        {
            if (string.IsNullOrWhiteSpace(modelName))
                return false;
                
            var isValid = GeminiModels.MultimodalModels.Contains(modelName);
            if (!isValid)
            {
                _loggingService.LogWarning($"Invalid multimodal model: {modelName}");
            }
            
            return isValid;
        }
        
        /// <inheritdoc/>
        public bool IsValidEmbeddingModel(string modelName)
        {
            if (string.IsNullOrWhiteSpace(modelName))
                return false;
                
            var isValid = GeminiModels.EmbeddingModels.Contains(modelName);
            if (!isValid)
            {
                _loggingService.LogWarning($"Invalid embedding model: {modelName}");
            }
            
            return isValid;
        }
        
        /// <inheritdoc/>
        public ValidationResult ValidateFunctionDeclaration(Model.Request.FunctionDeclaration functionDeclaration)
        {
            var errors = new List<string>();
            
            if (functionDeclaration == null)
            {
                errors.Add("Function declaration cannot be null");
                return new ValidationResult { IsValid = false, Errors = errors };
            }
            
            if (string.IsNullOrWhiteSpace(functionDeclaration.Name))
                errors.Add("Function name is required");
            
            if (string.IsNullOrWhiteSpace(functionDeclaration.Description))
                errors.Add("Function description is required");
            
            if (functionDeclaration.Parameters == null)
                errors.Add("Function parameters are required");
            else
            {
                if (string.IsNullOrWhiteSpace(functionDeclaration.Parameters.Type))
                    errors.Add("Function parameters type is required");
                
                if (functionDeclaration.Parameters.Properties == null || !functionDeclaration.Parameters.Properties.Any())
                    errors.Add("Function parameters must have at least one property");
                else
                {
                    foreach (var property in functionDeclaration.Parameters.Properties)
                    {
                        if (string.IsNullOrWhiteSpace(property.Key))
                            errors.Add("Property name cannot be empty");
                        
                        if (property.Value == null)
                            errors.Add($"Property '{property.Key}' cannot be null");
                        else
                        {
                            if (string.IsNullOrWhiteSpace(property.Value.Type))
                                errors.Add($"Property '{property.Key}' type is required");
                            
                            if (string.IsNullOrWhiteSpace(property.Value.Description))
                                errors.Add($"Property '{property.Key}' description is required");
                        }
                    }
                }
            }
            
            return new ValidationResult
            {
                IsValid = !errors.Any(),
                Errors = errors
            };
        }
        
        /// <inheritdoc/>
        public ValidationResult ValidateSystemInstruction(string systemInstruction)
        {
            var errors = new List<string>();
            
            if (string.IsNullOrWhiteSpace(systemInstruction))
                errors.Add("System instruction cannot be null or empty");
            else if (systemInstruction.Length > 10000)
                errors.Add("System instruction is too long (max 10,000 characters)");
            
            return new ValidationResult
            {
                IsValid = !errors.Any(),
                Errors = errors
            };
        }
        
        /// <inheritdoc/>
        public ValidationResult ValidateGenerationConfig(Model.Request.GenerationConfig generationConfig)
        {
            var errors = new List<string>();
            
            if (generationConfig == null)
                return new ValidationResult { IsValid = true, Errors = errors };
            
            if (generationConfig.Temperature < 0 || generationConfig.Temperature > 2)
                errors.Add("Temperature must be between 0 and 2");
            
            if (generationConfig.TopP < 0 || generationConfig.TopP > 1)
                errors.Add("TopP must be between 0 and 1");
            
            if (generationConfig.TopK < 0)
                errors.Add("TopK must be non-negative");
            
            if (generationConfig.MaxOutputTokens < 0)
                errors.Add("MaxOutputTokens must be non-negative");
            else if (generationConfig.MaxOutputTokens > 8192)
                errors.Add("MaxOutputTokens cannot exceed 8192");
            
            return new ValidationResult
            {
                IsValid = !errors.Any(),
                Errors = errors
            };
        }
        
        /// <inheritdoc/>
        public ValidationResult ValidateSafetySettings(IEnumerable<Model.Request.SafetySetting> safetySettings)
        {
            var errors = new List<string>();
            
            if (safetySettings == null)
                return new ValidationResult { IsValid = true, Errors = errors };
            
            var validCategories = new[] { "HARM_CATEGORY_HARASSMENT", "HARM_CATEGORY_HATE_SPEECH", "HARM_CATEGORY_SEXUALLY_EXPLICIT", "HARM_CATEGORY_DANGEROUS_CONTENT" };
            var validThresholds = new[] { "BLOCK_NONE", "BLOCK_ONLY_HIGH", "BLOCK_MEDIUM_AND_ABOVE", "BLOCK_LOW_AND_ABOVE" };
            
            foreach (var setting in safetySettings)
            {
                if (setting == null)
                {
                    errors.Add("Safety setting cannot be null");
                    continue;
                }
                
                if (string.IsNullOrWhiteSpace(setting.Category))
                    errors.Add("Safety setting category is required");
                else if (!validCategories.Contains(setting.Category))
                    errors.Add($"Invalid safety category: {setting.Category}");
                
                if (string.IsNullOrWhiteSpace(setting.Threshold))
                    errors.Add("Safety setting threshold is required");
                else if (!validThresholds.Contains(setting.Threshold))
                    errors.Add($"Invalid safety threshold: {setting.Threshold}");
            }
            
            return new ValidationResult
            {
                IsValid = !errors.Any(),
                Errors = errors
            };
        }
    }
    
    /// <summary>
    /// Validation result
    /// </summary>
    public class ValidationResult
    {
        /// <summary>
        /// Whether the validation passed
        /// </summary>
        public bool IsValid { get; set; }
        
        /// <summary>
        /// List of validation errors
        /// </summary>
        public List<string> Errors { get; set; } = new();
        
        /// <summary>
        /// Get error message
        /// </summary>
        /// <returns>Combined error message</returns>
        public string GetErrorMessage()
        {
            return string.Join("; ", Errors);
        }
    }
}

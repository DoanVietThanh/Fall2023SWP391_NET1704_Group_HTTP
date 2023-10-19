using Amazon.S3;
using Amazon.S3.Model;

namespace DriverLicenseLearningSupport.Services
{

    public interface IImageService 
    {
        Task<PutObjectResponse> UploadImageAsync(Guid id, IFormFile file);
        Task<GetObjectResponse?> GetImageAsync(Guid id);
        Task<string> GetPreSignedURL(Guid id);
        Task<DeleteObjectResponse?> DeleteImageAsync(Guid id);
    }

    public class ImageService : IImageService
    {
        private readonly IAmazonS3 _s3;
        private const string BucketName = "swp391-driverlicenselearningsupport";

        public ImageService(IAmazonS3 s3)
        {
            _s3 = s3;    
        }
        public async Task<PutObjectResponse> UploadImageAsync(Guid id, IFormFile file)
        {
            var awsAccessKeyId = "AKIAREKH23Z6WA54BHXA";
            var awsSecretAccessKey = "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";
            var awsCredentials = new Amazon.Runtime.BasicAWSCredentials(awsAccessKeyId, awsSecretAccessKey);
            var s3Config = new AmazonS3Config
            {
                RegionEndpoint = Amazon.RegionEndpoint.APSoutheast1
            };
            using (var s3Client = new AmazonS3Client(awsCredentials, s3Config))
            {
                // request
                var putObjectRequest = new PutObjectRequest
                {
                    BucketName = BucketName,
                    Key = $"profile_images/{id}",
                    ContentType = file.ContentType,
                    InputStream = file.OpenReadStream(),
                    Metadata =
                {
                    ["x-amz-meta-originalname"] = file.FileName,
                    ["x-amz-meta-extension"] = Path.GetExtension(file.FileName)
                }
                };
                // response        
                return await s3Client.PutObjectAsync(putObjectRequest);
            //return await _s3.PutObjectAsync(putObjectRequest);
        }

            return null;
        }

        public async Task<GetObjectResponse?> GetImageAsync(Guid id)
        {
            try
            {
                // request
                var getObjectRequest = new GetObjectRequest
                {
                    BucketName = BucketName,
                    Key = $"profile_images/{id}"
                };
                // response
                return await _s3.GetObjectAsync(getObjectRequest);
            }
            catch(AmazonS3Exception ex) when (ex.Message is "the specified key does not exist")
            {
                return null;
            }
        }

        public async Task<string> GetPreSignedURL(Guid id)
        {
            var awsAccessKeyId = "AKIAREKH23Z6WA54BHXA";
            var awsSecretAccessKey = "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";
            var awsCredentials = new Amazon.Runtime.BasicAWSCredentials(awsAccessKeyId, awsSecretAccessKey);

            var s3Config = new AmazonS3Config
            {
                RegionEndpoint = Amazon.RegionEndpoint.APSoutheast1
            };

            using(var s3Client = new AmazonS3Client(awsCredentials, s3Config))
            {
                var request = new GetPreSignedUrlRequest
                {
                    BucketName = BucketName,
                    Key = $"profile_images/{id}",
                    Expires = DateTime.Now.AddDays(1),
                    Verb = HttpVerb.GET
                };
                return s3Client.GetPreSignedURL(request);
            }
        }

        public async Task<DeleteObjectResponse?> DeleteImageAsync(Guid id)
        {
            try
            {
                // request
                var deleteObjectRequest = new DeleteObjectRequest
                {
                    BucketName = BucketName,
                    Key = $"profile_image/{id}"
                };
                // response
                return await _s3.DeleteObjectAsync(deleteObjectRequest);
            }
            catch (AmazonS3Exception ex) when (ex.Message is "the specified key doest not exist") 
            {
                return null;
            }
        }
    }
}

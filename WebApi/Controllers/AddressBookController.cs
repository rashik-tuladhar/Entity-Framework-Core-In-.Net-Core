using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.DataAccess.EntityFrameworkCore.DataModel;
using WebApi.Shared.Model;

namespace WebApi.Controllers
{
    [Route("api/addressbook")]
    [ApiController]
    public class AddressBookController : ControllerBase
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public AddressBookController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        /// <summary>
        /// Get All List In Address Book
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("getListContacts")]
        public async Task<List<Contact>> GetAllContacts()
        {
            return await _applicationDbContext.ContactDetails.ToListAsync();
        }


        /// <summary>
        /// Get Contact Details By First Name
        /// </summary>
        /// <param name="firstName"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("getContactByFirstName/{firstName}")]
        public async Task<List<Contact>> GetContactByFirstName(string firstName)
        {
            var contactDetails = await _applicationDbContext.ContactDetails.Where(x => x.FirstName == firstName).ToListAsync();
            return contactDetails;
        }


        /// <summary>
        /// Add Contact Details To Address Book
        /// </summary>
        /// <param name="contact"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("addContactDetails")]
        public async Task<HttpResponseMessage> AddContactDetails(Contact contact)
        {
            var response = new HttpResponseMessage(HttpStatusCode.NotFound);

            await _applicationDbContext.ContactDetails.AddAsync(contact);
            var result = await _applicationDbContext.SaveChangesAsync();
            if (result > 0)
            {
                response.StatusCode = HttpStatusCode.OK;
            }
            else
            {
                response.StatusCode = HttpStatusCode.BadRequest;
            }
            return response;
        }


        /// <summary>
        /// Update Contact Details Based In Id
        /// </summary>
        /// <param name="contact"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("updateContactDetails")]
        public async Task<HttpResponseMessage> UpdateContactDetails(Contact contact)
        {
            var response = new HttpResponseMessage(HttpStatusCode.BadRequest);
            var contactDetails = _applicationDbContext.ContactDetails.FirstOrDefault(x => x.Id == contact.Id);
            if (contactDetails != null)
            {
                contactDetails.FirstName = contact.FirstName;
                contactDetails.LastName = contact.LastName;
                contactDetails.Address = contact.Address;
                contactDetails.MobileNo = contact.MobileNo;
            }
            var result = await _applicationDbContext.SaveChangesAsync();
            if (result > 0)
            {
                response.StatusCode = HttpStatusCode.OK;
            }
            return response;
        }

        /// <summary>
        /// Remove A Contact Details By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("removeContactDetails/{id}")]
        public async Task<HttpResponseMessage> RemoveContactDetails(string id)
        {
            var response = new HttpResponseMessage(HttpStatusCode.BadRequest);
            Contact contact = new Contact{Id = id};
            _applicationDbContext.ContactDetails.Remove(contact);
            var result = await _applicationDbContext.SaveChangesAsync();
            if (result > 0)
            {
                response.StatusCode = HttpStatusCode.OK;
            }
            return response;
        }
    }
}

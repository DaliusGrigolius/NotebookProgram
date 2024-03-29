﻿using System.ComponentModel.DataAnnotations;

namespace NotebookProgram.Repository.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public byte[] PasswordHash { get; set; }
        [Required]
        public byte[] PasswordSalt { get; set; }
        public List<Note> Notes { get; set; }
        public List<RefreshToken> RefreshTokens { get; set; }

        public User(string username, byte[] passwordHash, byte[] passwordSalt)
        {
            Id = Guid.NewGuid();
            Username = username;
            PasswordHash = passwordHash;
            PasswordSalt = passwordSalt;
            Notes = new List<Note>();
            RefreshTokens = new List<RefreshToken>();
        }
    }
}

import { useState } from 'react'
import './App.css'

const BASE_URL = "http://localhost:5073";

function App() {
    const [mode, setMode] = useState<string>("");
    const [id, setId] = useState("");
    const [game, setGame] = useState<any>(null);
    const [games, setGames] = useState<any>([]);
    const [price, setPrice] = useState("");
    const [name, setName] = useState("");
    const [genreId, setGenreId] = useState("");
    const [releaseDate, setReleaseDate] = useState("");
       
    const getGameUsingID = async () => {
        const Game = await fetch(`${BASE_URL}/games/${id}`);
        const data = await Game.json();
        console.log(data);
        await setGame(data);
    };

    const getGames = async () => {
        const Games = await fetch(`${BASE_URL}/games`);
        const data = await Games.json();
        console.log(data);
        await setGames(data);
    };

    const addGame = async (name: string, price: string, genreId: string, releaseDate: string) => {
        const Game = await fetch(`${BASE_URL}/games`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({
                name: name,
                genreId: Number(genreId),
                price: Number(price),
                releaseDate: releaseDate
            })
        });
        
        console.log("STATUS:", Game.status);
        const data = await Game.text();
        console.log("RESPONSE:", data);
        await getGames();
        console.log("Added game");
    };

    const deleteGame = async (id: number) => {
        const Game = await fetch (`${BASE_URL}/games/${id}`, {            
            method: "DELETE"
        });

        //after deletion, you want the UI refreshes
        await getGames();
    };

    return (
        <div className="container">
            <h1>🎮 Game Finder</h1>
            <button onClick={() => setMode("get")}>Get</button>
            <button onClick={() => setMode("add")}>Add</button><button onClick={async () => {
                setMode("list"); // set the mode to show the list
                await getGames(); // fetch all games
            }}>
            Show All</button>

            {mode == "get" && (
                <>
                    <div className="search-section">
                        <input onChange={(e) => setId(e.target.value)} placeholder="Enter ID" />
                        <button onClick={getGameUsingID}>Get Game</button>
                    </div>

                    {game && (
                    <div className="card">
                        <h2>{game.name}</h2>
                        <p>₹ {game.price}</p>
                    </div>
                    )}
                </>
            )}

            {mode == "add" && (
                <>
                    <input placeholder="Name" onChange={(e) => setName(e.target.value)} />
                    <input placeholder="Price" onChange={(e) => setPrice(e.target.value)} />
                    <input placeholder="Genre ID" onChange={(e) => setGenreId(e.target.value)} />
                    <input placeholder="Release Date" onChange={(e) => setReleaseDate(e.target.value)} />
                    <button onClick={() => addGame(name, price, genreId, releaseDate)}>Add Game</button>
                </>                
            )}

            {mode == "list" && (
                <div className="all-games">

                <div className="games-list">
                    {games.map((g: any) => (
                    <div className="card" key={g.id}>
                        <h3>{g.name}</h3>
                        <p>₹ {g.price}</p>

                        <button onClick={() => deleteGame(g.id)}>Delete</button>
                    </div>
                    ))}
                </div>
                </div>
            )}
        </div>
        );
}

export default App

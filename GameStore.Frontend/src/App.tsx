import { useState } from 'react'
import './App.css'

const BASE_URL = "http://localhost:5073";

function App() {
    const [id, setId] = useState("");
    const [game, setGame] = useState<any>(null);
    const [games, setGames] = useState<any>([]);
       
    const getGameUsingID = async () => {
        const Game = await fetch(`${BASE_URL}/games/${id}`);
        const data = await Game.json();
        console.log(data);
        setGame(data);
    };

    const getGames = async () => {
        const Games = await fetch(`${BASE_URL}/games`);
        const data = await Games.json();
        console.log(data);
        setGames(data);
    }

    return (
        <div className="container">
            
            <h1>🎮 Game Finder</h1>

            <div className="search-section">
            <input
                onChange={(e) => setId(e.target.value)}
                placeholder="Enter ID"
            />
            <button onClick={getGameUsingID}>Get Game</button>
            </div>

            {game && (
            <div className="card">
                <h2>{game.name}</h2>
                <p>₹ {game.price}</p>
            </div>
            )}

            <div className="all-games">
            <button onClick={getGames}>Get All Games</button>

            <div className="games-list">
                {games.map((g: any) => (
                <div className="card" key={g.id}>
                    <h3>{g.name}</h3>
                    <p>₹ {g.price}</p>
                </div>
                ))}
            </div>
            </div>

        </div>
        );
}

export default App
